using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap
{
    public class RoomSettings
    {
        public RoomType roomtype;

        public string caption; //Room Name
        public string owner; //User Id of the Room Owner
        public string description; //Room Description

        public string model_name; //We can get FloorPlan form Database

        public string category;
        public string state;
        public string users_now;
        public string users_max;

        public string score;
        public string tags;
        public string password;

        public string wallpaper, floor, landscape;

        public string allow_pets, allow_pets_eat;
        public string room_blocking_disabled, allow_hidewall;

        public string wallthick, floorthick;
        public string group_id;

        public string mute_settings, ban_settings, kick_settings;
        public string chat_mode, chat_size, chat_speed, chat_extra_flood, chat_hearing_distance;

        public string trade_settings;

        public string spush_enabled, push_enabled, spull_enabled, pull_enabled, enables_enabled;
        public string respect_notifications_enabled;
        public string pet_morphs_allowed, golpe_enabled;

        public string tutorial;

        public RoomSettings(Dictionary<string, string> list)
        {
            list.TryGetValue("caption", out caption);
            list.TryGetValue("owner", out owner);
            list.TryGetValue("description", out description);
            list.TryGetValue("model_name", out model_name);
            list.TryGetValue("category", out category);
            list.TryGetValue("state", out state);
            list.TryGetValue("users_now", out users_now);
            list.TryGetValue("users_max", out users_max);
            list.TryGetValue("score", out score);
            list.TryGetValue("tags", out tags);
            list.TryGetValue("password", out password);
            list.TryGetValue("wallpaper", out wallpaper);
            list.TryGetValue("floor", out floor);
            list.TryGetValue("landscape", out landscape);
            list.TryGetValue("allow_pets", out allow_pets);
            list.TryGetValue("allow_pets_eat", out allow_pets_eat);
            list.TryGetValue("room_blocking_disabled", out room_blocking_disabled);
            list.TryGetValue("allow_hidewall", out allow_hidewall);
            list.TryGetValue("wallthick", out wallthick);
            list.TryGetValue("floorthick", out floorthick);
            list.TryGetValue("group_id", out group_id);
            list.TryGetValue("mute_settings", out mute_settings);
            list.TryGetValue("ban_settings", out ban_settings);
            list.TryGetValue("kick_settings", out kick_settings);
            list.TryGetValue("chat_mode", out chat_mode);
            list.TryGetValue("chat_size", out chat_size);
            list.TryGetValue("chat_speed", out chat_speed);
            list.TryGetValue("chat_extra_flood", out chat_extra_flood);
            list.TryGetValue("chat_hearing_distance", out chat_hearing_distance);
            list.TryGetValue("trade_settings", out trade_settings);
            list.TryGetValue("spush_enabled", out spush_enabled);
            list.TryGetValue("push_enabled", out push_enabled);
            list.TryGetValue("spull_enabled", out spull_enabled);
            list.TryGetValue("pull_enabled", out pull_enabled);
            list.TryGetValue("enables_enabled", out enables_enabled);
            list.TryGetValue("respect_notifications_enabled", out respect_notifications_enabled);
            list.TryGetValue("pet_morphs_allowed", out pet_morphs_allowed);
            list.TryGetValue("golpe_enabled", out golpe_enabled);
            list.TryGetValue("tutorial", out tutorial);
        }
    }
}
