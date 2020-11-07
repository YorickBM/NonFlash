#pragma once
#include <string>
#include "CImg.h"

#include <map>
#include <unordered_map>
#include <string>
#include <vector>

#include <glm/glm.hpp>

#include "CollisionExtra.h"
#include "Model.h"
#include "ToolBox.h"
#include "ComponentSystemManager.h"

using namespace cimg_library;

class Chunk
{
public:
	ComponentSystemManager csm;

	Chunk(Chunk &chunk) {
		std::cout << "Using Refrence Struct" << std::endl;
	}

	~Chunk() {}
	Chunk(glm::vec2 position, const char *modelPath) {
		Model model(modelPath, glm::vec3(position.x, 0, position.y), 1.f);
		chunkModel.push_back(model);

		auto chunkEntity = csm.CreateEntity();
		csm.AddComponent(chunkEntity, RenderObjectC{ model });
		csm.AddComponent(chunkEntity, NonEntityC{});
	}	

	/*float GetHeight(glm::vec3 position) {
		Model model = chunkModel.at(chunkModel.size() - 1);
		float x = (int)position.x + .5f;
		float z = (int)position.z + .5f;
		float y = 0;
		float midY = 0;

		glm::vec3 FirstCornor, PrevCorner = glm::vec3(0, 0, 0);
		for (glm::vec3 corner : model.getCubeCorners(glm::vec2(x, z))) {
			//Loops 4 times (each corner once)
			//Distance between 2 corners is 1 (its a square of 1x1)
			if (FirstCornor == glm::vec3(0, 0, 0)) {
				FirstCornor = corner;
				break;
			}

			midY += FloatOnLine(PrevCorner, corner, position, (PrevCorner.z == corner.z));

			PrevCorner = corner;
		}
		midY += FloatOnLine(FirstCornor, PrevCorner, position, (FirstCornor.z == PrevCorner.z));

		y = midY / 4;

		return y;
	}//*/

private:
	glm::vec3 Position;
	std::vector<Model> chunkModel;

	/*float FloatOnLine(glm::vec3 prevCorner, glm::vec3 corner, glm::vec2 position, bool UseZCoord = false) {
		float PositionOnLine = 0;

		if (!UseZCoord) PositionOnLine = (prevCorner.x + corner.x) / position.x;
		else if (UseZCoord) PositionOnLine = (prevCorner.z + corner.z) / position.y;

		return ((prevCorner.y + corner.y) / PositionOnLine);
	}//*/
};

class ChunkManager {
public:
	/*Chunk GetChunk(glm::vec2 position) {
		if (chunks.find(position) != chunks.end()) {
			return chunks.at(position);
		}
	}
	Chunk GetChunk(glm::vec3 position) {
		if (chunks.find(glm::vec2((int)position.x / 16, (int)position.z / 16)) != chunks.end()) {
			return chunks.at(position);
		}
	}//*/

	void AddChunk(glm::vec2 location, Chunk chunk) {
		if (chunks.find(location) == chunks.end()) {
			chunks.insert_or_assign(location, chunk);
		}
		else {
			//Throw error - Chunk already exists
		}
	}
	void InitializeChunks(const char* pathImage) {
		CImg<unsigned char> src(pathImage);
		int width = src.width();
		int height = src.height();

		int ChunkId = 0;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; x++) {
				unsigned char* ptr = src.data(x, y); // get pointer to pixel @ x,y

				//ChunkId = ptr->ColorR (Get Used Color(s) from config.ini)
				ChunkId += (int)src.data(x, y, 0, 0);
				ChunkId += (int)src.data(x, y, 0, 1);
				ChunkId += (int)src.data(x, y, 0, 2);

				//Generate Position on Size & X & Y from loops
				glm::vec2 chunkPosition(x, MAX_CHUNKS_Y - y);

				//Create Chunk
				Chunk newChunk(chunkPosition, "res/System/Chunks/Chunk_" + ChunkId);
				AddChunk(chunkPosition, newChunk);
			}
		}
	}

private:
	std::map<glm::vec2, Chunk> chunks;
	const int MAX_CHUNKS_Y = 1; //Image Size Y - 1
};

