/*
 * Copyright (c) Contributors, http://aurora-sim.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Aurora-Sim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using vector = Aurora.ScriptEngine.AuroraDotNetEngine.LSL_Types.Vector3;
using rotation = Aurora.ScriptEngine.AuroraDotNetEngine.LSL_Types.Quaternion;
using LSLInteger = Aurora.ScriptEngine.AuroraDotNetEngine.LSL_Types.LSLInteger;
using Aurora.ScriptEngine.AuroraDotNetEngine;
using Aurora.ScriptEngine.AuroraDotNetEngine.APIs.Interfaces;
using Aurora.ScriptEngine.AuroraDotNetEngine.CompilerTools;

namespace Aurora.ScriptEngine.AuroraDotNetEngine.Runtime
{
    public partial class ScriptBaseClass
    {
        // LSL CONSTANTS
        public static readonly LSLInteger TRUE = new LSLInteger(1);
        public static readonly LSLInteger FALSE = new LSLInteger(0);

        public static readonly LSLInteger STATUS_PHYSICS = 1;
        public static readonly LSLInteger STATUS_ROTATE_X = 2;
        public static readonly LSLInteger STATUS_ROTATE_Y = 4;
        public static readonly LSLInteger STATUS_ROTATE_Z = 8;
        public static readonly LSLInteger STATUS_PHANTOM = 16;
        public static readonly LSLInteger STATUS_SANDBOX = 32;
        public static readonly LSLInteger STATUS_BLOCK_GRAB = 64;
        public static readonly LSLInteger STATUS_DIE_AT_EDGE = 128;
        public static readonly LSLInteger STATUS_RETURN_AT_EDGE = 256;
        public static readonly LSLInteger STATUS_CAST_SHADOWS = 512;

        public static readonly LSLInteger AGENT = 1;
        public static readonly LSLInteger AGENT_BY_LEGACY_NAME = 1;
        public static readonly LSLInteger AGENT_BY_USERNAME = 0x10;
        public static readonly LSLInteger ACTIVE = 2;
        public static readonly LSLInteger PASSIVE = 4;
        public static readonly LSLInteger SCRIPTED = 8;

        public static readonly LSLInteger CONTROL_FWD = 1;
        public static readonly LSLInteger CONTROL_BACK = 2;
        public static readonly LSLInteger CONTROL_LEFT = 4;
        public static readonly LSLInteger CONTROL_RIGHT = 8;
        public static readonly LSLInteger CONTROL_UP = 16;
        public static readonly LSLInteger CONTROL_DOWN = 32;
        public static readonly LSLInteger CONTROL_ROT_LEFT = 256;
        public static readonly LSLInteger CONTROL_ROT_RIGHT = 512;
        public static readonly LSLInteger CONTROL_LBUTTON = 268435456;
        public static readonly LSLInteger CONTROL_ML_LBUTTON = 1073741824;

        //Permissions
        public static readonly LSLInteger PERMISSION_DEBIT = 2;
        public static readonly LSLInteger PERMISSION_TAKE_CONTROLS = 4;
        public static readonly LSLInteger PERMISSION_REMAP_CONTROLS = 8;
        public static readonly LSLInteger PERMISSION_TRIGGER_ANIMATION = 16;
        public static readonly LSLInteger PERMISSION_ATTACH = 32;
        public static readonly LSLInteger PERMISSION_RELEASE_OWNERSHIP = 64;
        public static readonly LSLInteger PERMISSION_CHANGE_LINKS = 128;
        public static readonly LSLInteger PERMISSION_CHANGE_JOINTS = 256;
        public static readonly LSLInteger PERMISSION_CHANGE_PERMISSIONS = 512;
        public static readonly LSLInteger PERMISSION_TRACK_CAMERA = 1024;
        public static readonly LSLInteger PERMISSION_CONTROL_CAMERA = 2048;
        public static readonly LSLInteger PERMISSION_COMBAT = 8196;

        public static readonly LSLInteger AGENT_FLYING = 1;
        public static readonly LSLInteger AGENT_ATTACHMENTS = 2;
        public static readonly LSLInteger AGENT_SCRIPTED = 4;
        public static readonly LSLInteger AGENT_MOUSELOOK = 8;
        public static readonly LSLInteger AGENT_SITTING = 16;
        public static readonly LSLInteger AGENT_ON_OBJECT = 32;
        public static readonly LSLInteger AGENT_AWAY = 64;
        public static readonly LSLInteger AGENT_WALKING = 128;
        public static readonly LSLInteger AGENT_IN_AIR = 256;
        public static readonly LSLInteger AGENT_TYPING = 512;
        public static readonly LSLInteger AGENT_CROUCHING = 1024;
        public static readonly LSLInteger AGENT_BUSY = 2048;
        public static readonly LSLInteger AGENT_ALWAYS_RUN = 4096;

        //Particle Systems
        public static readonly LSLInteger PSYS_PART_INTERP_COLOR_MASK = 1;
        public static readonly LSLInteger PSYS_PART_INTERP_SCALE_MASK = 2;
        public static readonly LSLInteger PSYS_PART_BOUNCE_MASK = 4;
        public static readonly LSLInteger PSYS_PART_WIND_MASK = 8;
        public static readonly LSLInteger PSYS_PART_FOLLOW_SRC_MASK = 16;
        public static readonly LSLInteger PSYS_PART_FOLLOW_VELOCITY_MASK = 32;
        public static readonly LSLInteger PSYS_PART_TARGET_POS_MASK = 64;
        public static readonly LSLInteger PSYS_PART_TARGET_LINEAR_MASK = 128;
        public static readonly LSLInteger PSYS_PART_EMISSIVE_MASK = 256;
        public static readonly LSLInteger PSYS_PART_FLAGS = 0;
        public static readonly LSLInteger PSYS_PART_START_COLOR = 1;
        public static readonly LSLInteger PSYS_PART_START_ALPHA = 2;
        public static readonly LSLInteger PSYS_PART_END_COLOR = 3;
        public static readonly LSLInteger PSYS_PART_END_ALPHA = 4;
        public static readonly LSLInteger PSYS_PART_START_SCALE = 5;
        public static readonly LSLInteger PSYS_PART_END_SCALE = 6;
        public static readonly LSLInteger PSYS_PART_MAX_AGE = 7;
        public static readonly LSLInteger PSYS_SRC_ACCEL = 8;
        public static readonly LSLInteger PSYS_SRC_PATTERN = 9;
        public static readonly LSLInteger PSYS_SRC_INNERANGLE = 10;
        public static readonly LSLInteger PSYS_SRC_OUTERANGLE = 11;
        public static readonly LSLInteger PSYS_SRC_TEXTURE = 12;
        public static readonly LSLInteger PSYS_SRC_BURST_RATE = 13;
        public static readonly LSLInteger PSYS_SRC_BURST_PART_COUNT = 15;
        public static readonly LSLInteger PSYS_SRC_BURST_RADIUS = 16;
        public static readonly LSLInteger PSYS_SRC_BURST_SPEED_MIN = 17;
        public static readonly LSLInteger PSYS_SRC_BURST_SPEED_MAX = 18;
        public static readonly LSLInteger PSYS_SRC_MAX_AGE = 19;
        public static readonly LSLInteger PSYS_SRC_TARGET_KEY = 20;
        public static readonly LSLInteger PSYS_SRC_OMEGA = 21;
        public static readonly LSLInteger PSYS_SRC_ANGLE_BEGIN = 22;
        public static readonly LSLInteger PSYS_SRC_ANGLE_END = 23;
        public static readonly LSLInteger PSYS_SRC_PATTERN_DROP = 1;
        public static readonly LSLInteger PSYS_SRC_PATTERN_EXPLODE = 2;
        public static readonly LSLInteger PSYS_SRC_PATTERN_ANGLE = 4;
        public static readonly LSLInteger PSYS_SRC_PATTERN_ANGLE_CONE = 8;
        public static readonly LSLInteger PSYS_SRC_PATTERN_ANGLE_CONE_EMPTY = 16;

        public static readonly LSLInteger VEHICLE_TYPE_NONE = 0;
        public static readonly LSLInteger VEHICLE_TYPE_SLED = 1;
        public static readonly LSLInteger VEHICLE_TYPE_CAR = 2;
        public static readonly LSLInteger VEHICLE_TYPE_BOAT = 3;
        public static readonly LSLInteger VEHICLE_TYPE_AIRPLANE = 4;
        public static readonly LSLInteger VEHICLE_TYPE_BALLOON = 5;
        public static readonly LSLInteger VEHICLE_LINEAR_FRICTION_TIMESCALE = 16;
        public static readonly LSLInteger VEHICLE_ANGULAR_FRICTION_TIMESCALE = 17;
        public static readonly LSLInteger VEHICLE_LINEAR_MOTOR_DIRECTION = 18;
        public static readonly LSLInteger VEHICLE_LINEAR_MOTOR_OFFSET = 20;
        public static readonly LSLInteger VEHICLE_ANGULAR_MOTOR_DIRECTION = 19;
        public static readonly LSLInteger VEHICLE_HOVER_HEIGHT = 24;
        public static readonly LSLInteger VEHICLE_HOVER_EFFICIENCY = 25;
        public static readonly LSLInteger VEHICLE_HOVER_TIMESCALE = 26;
        public static readonly LSLInteger VEHICLE_BUOYANCY = 27;
        public static readonly LSLInteger VEHICLE_LINEAR_DEFLECTION_EFFICIENCY = 28;
        public static readonly LSLInteger VEHICLE_LINEAR_DEFLECTION_TIMESCALE = 29;
        public static readonly LSLInteger VEHICLE_LINEAR_MOTOR_TIMESCALE = 30;
        public static readonly LSLInteger VEHICLE_LINEAR_MOTOR_DECAY_TIMESCALE = 31;
        public static readonly LSLInteger VEHICLE_ANGULAR_DEFLECTION_EFFICIENCY = 32;
        public static readonly LSLInteger VEHICLE_ANGULAR_DEFLECTION_TIMESCALE = 33;
        public static readonly LSLInteger VEHICLE_ANGULAR_MOTOR_TIMESCALE = 34;
        public static readonly LSLInteger VEHICLE_ANGULAR_MOTOR_DECAY_TIMESCALE = 35;
        public static readonly LSLInteger VEHICLE_VERTICAL_ATTRACTION_EFFICIENCY = 36;
        public static readonly LSLInteger VEHICLE_VERTICAL_ATTRACTION_TIMESCALE = 37;
        public static readonly LSLInteger VEHICLE_BANKING_EFFICIENCY = 38;
        public static readonly LSLInteger VEHICLE_BANKING_MIX = 39;
        public static readonly LSLInteger VEHICLE_BANKING_TIMESCALE = 40;
        public static readonly LSLInteger VEHICLE_REFERENCE_FRAME = 44;
        public static readonly LSLInteger VEHICLE_RANGE_BLOCK = 45;
        public static readonly LSLInteger VEHICLE_ROLL_FRAME = 46;
        public static readonly LSLInteger VEHICLE_FLAG_NO_DEFLECTION_UP = 1;
        public static readonly LSLInteger VEHICLE_FLAG_NO_FLY_UP = 1;//Old name for NO_DEFLECTION_UP
        public static readonly LSLInteger VEHICLE_FLAG_LIMIT_ROLL_ONLY = 2;
        public static readonly LSLInteger VEHICLE_FLAG_HOVER_WATER_ONLY = 4;
        public static readonly LSLInteger VEHICLE_FLAG_HOVER_TERRAIN_ONLY = 8;
        public static readonly LSLInteger VEHICLE_FLAG_HOVER_GLOBAL_HEIGHT = 16;
        public static readonly LSLInteger VEHICLE_FLAG_HOVER_UP_ONLY = 32;
        public static readonly LSLInteger VEHICLE_FLAG_LIMIT_MOTOR_UP = 64;
        public static readonly LSLInteger VEHICLE_FLAG_MOUSELOOK_STEER = 128;
        public static readonly LSLInteger VEHICLE_FLAG_MOUSELOOK_BANK = 256;
        public static readonly LSLInteger VEHICLE_FLAG_CAMERA_DECOUPLED = 512;
        public static readonly LSLInteger VEHICLE_FLAG_NO_X = 1024;
        public static readonly LSLInteger VEHICLE_FLAG_NO_Y = 2048;
        public static readonly LSLInteger VEHICLE_FLAG_NO_Z = 4096;
        public static readonly LSLInteger VEHICLE_FLAG_LOCK_HOVER_HEIGHT = 8192;
        public static readonly LSLInteger VEHICLE_FLAG_NO_DEFLECTION = 16392;
        public static readonly LSLInteger VEHICLE_FLAG_LOCK_ROTATION = 32784;

        public static readonly LSLInteger INVENTORY_ALL = -1;
        public static readonly LSLInteger INVENTORY_NONE = -1;
        public static readonly LSLInteger INVENTORY_TEXTURE = 0;
        public static readonly LSLInteger INVENTORY_SOUND = 1;
        public static readonly LSLInteger INVENTORY_LANDMARK = 3;
        public static readonly LSLInteger INVENTORY_CLOTHING = 5;
        public static readonly LSLInteger INVENTORY_OBJECT = 6;
        public static readonly LSLInteger INVENTORY_NOTECARD = 7;
        public static readonly LSLInteger INVENTORY_SCRIPT = 10;
        public static readonly LSLInteger INVENTORY_BODYPART = 13;
        public static readonly LSLInteger INVENTORY_ANIMATION = 20;
        public static readonly LSLInteger INVENTORY_GESTURE = 21;

        public static readonly LSLInteger ATTACH_CHEST = 1;
        public static readonly LSLInteger ATTACH_HEAD = 2;
        public static readonly LSLInteger ATTACH_LSHOULDER = 3;
        public static readonly LSLInteger ATTACH_RSHOULDER = 4;
        public static readonly LSLInteger ATTACH_LHAND = 5;
        public static readonly LSLInteger ATTACH_RHAND = 6;
        public static readonly LSLInteger ATTACH_LFOOT = 7;
        public static readonly LSLInteger ATTACH_RFOOT = 8;
        public static readonly LSLInteger ATTACH_BACK = 9;
        public static readonly LSLInteger ATTACH_PELVIS = 10;
        public static readonly LSLInteger ATTACH_MOUTH = 11;
        public static readonly LSLInteger ATTACH_CHIN = 12;
        public static readonly LSLInteger ATTACH_LEAR = 13;
        public static readonly LSLInteger ATTACH_REAR = 14;
        public static readonly LSLInteger ATTACH_LEYE = 15;
        public static readonly LSLInteger ATTACH_REYE = 16;
        public static readonly LSLInteger ATTACH_NOSE = 17;
        public static readonly LSLInteger ATTACH_RUARM = 18;
        public static readonly LSLInteger ATTACH_RLARM = 19;
        public static readonly LSLInteger ATTACH_LUARM = 20;
        public static readonly LSLInteger ATTACH_LLARM = 21;
        public static readonly LSLInteger ATTACH_RHIP = 22;
        public static readonly LSLInteger ATTACH_RULEG = 23;
        public static readonly LSLInteger ATTACH_RLLEG = 24;
        public static readonly LSLInteger ATTACH_LHIP = 25;
        public static readonly LSLInteger ATTACH_LULEG = 26;
        public static readonly LSLInteger ATTACH_LLLEG = 27;
        public static readonly LSLInteger ATTACH_BELLY = 28;
        public static readonly LSLInteger ATTACH_RPEC = 29;
        public static readonly LSLInteger ATTACH_LPEC = 30;
        public static readonly LSLInteger ATTACH_HUD_CENTER_2 = 31;
        public static readonly LSLInteger ATTACH_HUD_TOP_RIGHT = 32;
        public static readonly LSLInteger ATTACH_HUD_TOP_CENTER = 33;
        public static readonly LSLInteger ATTACH_HUD_TOP_LEFT = 34;
        public static readonly LSLInteger ATTACH_HUD_CENTER_1 = 35;
        public static readonly LSLInteger ATTACH_HUD_BOTTOM_LEFT = 36;
        public static readonly LSLInteger ATTACH_HUD_BOTTOM = 37;
        public static readonly LSLInteger ATTACH_HUD_BOTTOM_RIGHT = 38;

        public static readonly LSLInteger LAND_LEVEL = 0;
        public static readonly LSLInteger LAND_RAISE = 1;
        public static readonly LSLInteger LAND_LOWER = 2;
        public static readonly LSLInteger LAND_SMOOTH = 3;
        public static readonly LSLInteger LAND_NOISE = 4;
        public static readonly LSLInteger LAND_REVERT = 5;
        public static readonly LSLInteger LAND_SMALL_BRUSH = 1;
        public static readonly LSLInteger LAND_MEDIUM_BRUSH = 2;
        public static readonly LSLInteger LAND_LARGE_BRUSH = 3;

        //Agent Dataserver
        public static readonly LSLInteger DATA_ONLINE = 1;
        public static readonly LSLInteger DATA_NAME = 2;
        public static readonly LSLInteger DATA_BORN = 3;
        public static readonly LSLInteger DATA_RATING = 4;
        public static readonly LSLInteger DATA_SIM_POS = 5;
        public static readonly LSLInteger DATA_SIM_STATUS = 6;
        public static readonly LSLInteger DATA_SIM_RATING = 7;
        public static readonly LSLInteger DATA_PAYINFO = 8;
        public static readonly LSLInteger DATA_SIM_RELEASE = 128;

        public static readonly LSLInteger ANIM_ON = 1;
        public static readonly LSLInteger LOOP = 2;
        public static readonly LSLInteger REVERSE = 4;
        public static readonly LSLInteger PING_PONG = 8;
        public static readonly LSLInteger SMOOTH = 16;
        public static readonly LSLInteger ROTATE = 32;
        public static readonly LSLInteger SCALE = 64;
        public static readonly LSLInteger ALL_SIDES = -1;
        public static readonly LSLInteger LINK_SET = -1;
        public static readonly LSLInteger LINK_ROOT = 1;
        public static readonly LSLInteger LINK_ALL_OTHERS = -2;
        public static readonly LSLInteger LINK_ALL_CHILDREN = -3;
        public static readonly LSLInteger LINK_THIS = -4;
        public static readonly LSLInteger CHANGED_INVENTORY = 1;
        public static readonly LSLInteger CHANGED_COLOR = 2;
        public static readonly LSLInteger CHANGED_SHAPE = 4;
        public static readonly LSLInteger CHANGED_SCALE = 8;
        public static readonly LSLInteger CHANGED_TEXTURE = 16;
        public static readonly LSLInteger CHANGED_LINK = 32;
        public static readonly LSLInteger CHANGED_ALLOWED_DROP = 64;
        public static readonly LSLInteger CHANGED_OWNER = 128;
        public static readonly LSLInteger CHANGED_REGION = 256;
        public static readonly LSLInteger CHANGED_TELEPORT = 512;
        public static readonly LSLInteger CHANGED_REGION_RESTART = 1024;
        public static readonly LSLInteger CHANGED_REGION_START = 1024; //LL Changed the constant from CHANGED_REGION_RESTART
        public static readonly LSLInteger CHANGED_MEDIA = 2048;
        public static readonly LSLInteger CHANGED_ANIMATION = 16384;
        public static readonly LSLInteger CHANGED_STATE = 32768;
        public static readonly LSLInteger TYPE_INVALID = 0;
        public static readonly LSLInteger TYPE_INTEGER = 1;
        public static readonly LSLInteger TYPE_FLOAT = 2;
        public static readonly LSLInteger TYPE_STRING = 3;
        public static readonly LSLInteger TYPE_KEY = 4;
        public static readonly LSLInteger TYPE_VECTOR = 5;
        public static readonly LSLInteger TYPE_ROTATION = 6;

        //XML RPC Remote Data Channel
        public static readonly LSLInteger REMOTE_DATA_CHANNEL = 1;
        public static readonly LSLInteger REMOTE_DATA_REQUEST = 2;
        public static readonly LSLInteger REMOTE_DATA_REPLY = 3;

        //llHTTPRequest
        public static readonly LSLInteger HTTP_METHOD = 0;
        public static readonly LSLInteger HTTP_MIMETYPE = 1;
        public static readonly LSLInteger HTTP_BODY_MAXLENGTH = 2;
        public static readonly LSLInteger HTTP_VERIFY_CERT = 3;

        public static readonly LSLInteger PRIM_MATERIAL = 2;
        public static readonly LSLInteger PRIM_PHYSICS = 3;
        public static readonly LSLInteger PRIM_TEMP_ON_REZ = 4;
        public static readonly LSLInteger PRIM_PHANTOM = 5;
        public static readonly LSLInteger PRIM_POSITION = 6;
        public static readonly LSLInteger PRIM_SIZE = 7;
        public static readonly LSLInteger PRIM_ROTATION = 8;
        public static readonly LSLInteger PRIM_TYPE = 9;
        public static readonly LSLInteger PRIM_TEXTURE = 17;
        public static readonly LSLInteger PRIM_COLOR = 18;
        public static readonly LSLInteger PRIM_BUMP_SHINY = 19;
        public static readonly LSLInteger PRIM_FULLBRIGHT = 20;
        public static readonly LSLInteger PRIM_FLEXIBLE = 21;
        public static readonly LSLInteger PRIM_TEXGEN = 22;
        public static readonly LSLInteger PRIM_POINT_LIGHT = 23; 
        public static readonly LSLInteger PRIM_CAST_SHADOWS = 24; // Not implemented, here for completeness sake
        public static readonly LSLInteger PRIM_GLOW = 25;
        public static readonly LSLInteger PRIM_TEXT = 26;
        public static readonly LSLInteger PRIM_NAME = 27;
        public static readonly LSLInteger PRIM_DESC = 28;
        public static readonly LSLInteger PRIM_ROT_LOCAL = 29;
        public static readonly LSLInteger PRIM_OMEGA = 32;
        public static readonly LSLInteger PRIM_TEXGEN_DEFAULT = 0;
        public static readonly LSLInteger PRIM_TEXGEN_PLANAR = 1;

        public static readonly LSLInteger PRIM_TYPE_BOX = 0;
        public static readonly LSLInteger PRIM_TYPE_CYLINDER = 1;
        public static readonly LSLInteger PRIM_TYPE_PRISM = 2;
        public static readonly LSLInteger PRIM_TYPE_SPHERE = 3;
        public static readonly LSLInteger PRIM_TYPE_TORUS = 4;
        public static readonly LSLInteger PRIM_TYPE_TUBE = 5;
        public static readonly LSLInteger PRIM_TYPE_RING = 6;
        public static readonly LSLInteger PRIM_TYPE_SCULPT = 7;

        public static readonly LSLInteger PRIM_HOLE_DEFAULT = 0;
        public static readonly LSLInteger PRIM_HOLE_CIRCLE = 16;
        public static readonly LSLInteger PRIM_HOLE_SQUARE = 32;
        public static readonly LSLInteger PRIM_HOLE_TRIANGLE = 48;

        public static readonly LSLInteger PRIM_MATERIAL_STONE = 0;
        public static readonly LSLInteger PRIM_MATERIAL_METAL = 1;
        public static readonly LSLInteger PRIM_MATERIAL_GLASS = 2;
        public static readonly LSLInteger PRIM_MATERIAL_WOOD = 3;
        public static readonly LSLInteger PRIM_MATERIAL_FLESH = 4;
        public static readonly LSLInteger PRIM_MATERIAL_PLASTIC = 5;
        public static readonly LSLInteger PRIM_MATERIAL_RUBBER = 6;
        public static readonly LSLInteger PRIM_MATERIAL_LIGHT = 7;

        public static readonly LSLInteger PRIM_SHINY_NONE = 0;
        public static readonly LSLInteger PRIM_SHINY_LOW = 1;
        public static readonly LSLInteger PRIM_SHINY_MEDIUM = 2;
        public static readonly LSLInteger PRIM_SHINY_HIGH = 3;
        public static readonly LSLInteger PRIM_BUMP_NONE = 0;
        public static readonly LSLInteger PRIM_BUMP_BRIGHT = 1;
        public static readonly LSLInteger PRIM_BUMP_DARK = 2;
        public static readonly LSLInteger PRIM_BUMP_WOOD = 3;
        public static readonly LSLInteger PRIM_BUMP_BARK = 4;
        public static readonly LSLInteger PRIM_BUMP_BRICKS = 5;
        public static readonly LSLInteger PRIM_BUMP_CHECKER = 6;
        public static readonly LSLInteger PRIM_BUMP_CONCRETE = 7;
        public static readonly LSLInteger PRIM_BUMP_TILE = 8;
        public static readonly LSLInteger PRIM_BUMP_STONE = 9;
        public static readonly LSLInteger PRIM_BUMP_DISKS = 10;
        public static readonly LSLInteger PRIM_BUMP_GRAVEL = 11;
        public static readonly LSLInteger PRIM_BUMP_BLOBS = 12;
        public static readonly LSLInteger PRIM_BUMP_SIDING = 13;
        public static readonly LSLInteger PRIM_BUMP_LARGETILE = 14;
        public static readonly LSLInteger PRIM_BUMP_STUCCO = 15;
        public static readonly LSLInteger PRIM_BUMP_SUCTION = 16;
        public static readonly LSLInteger PRIM_BUMP_WEAVE = 17;

        public static readonly LSLInteger PRIM_SCULPT_TYPE_SPHERE = 1;
        public static readonly LSLInteger PRIM_SCULPT_TYPE_TORUS = 2;
        public static readonly LSLInteger PRIM_SCULPT_TYPE_PLANE = 3;
        public static readonly LSLInteger PRIM_SCULPT_TYPE_CYLINDER = 4;

        public static readonly LSLInteger MASK_BASE = 0;
        public static readonly LSLInteger MASK_OWNER = 1;
        public static readonly LSLInteger MASK_GROUP = 2;
        public static readonly LSLInteger MASK_EVERYONE = 3;
        public static readonly LSLInteger MASK_NEXT = 4;

        public static readonly LSLInteger PERM_TRANSFER = 8192;
        public static readonly LSLInteger PERM_MODIFY = 16384;
        public static readonly LSLInteger PERM_COPY = 32768;
        public static readonly LSLInteger PERM_MOVE = 524288;
        public static readonly LSLInteger PERM_ALL = 2147483647;

        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_STOP = 0;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_PAUSE = 1;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_PLAY = 2;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_LOOP = 3;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_TEXTURE = 4;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_URL = 5;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_TIME = 6;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_AGENT = 7;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_UNLOAD = 8;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_AUTO_ALIGN = 9;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_TYPE = 10;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_SIZE = 11;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_DESC = 12;
        public static readonly LSLInteger PARCEL_MEDIA_COMMAND_LOOP_SET = 13;

        // constants for llGetPrimMediaParams/llSetPrimMediaParams
        public static readonly LSLInteger PRIM_MEDIA_ALT_IMAGE_ENABLE = 0;
        public static readonly LSLInteger PRIM_MEDIA_CONTROLS = 1;
        public static readonly LSLInteger PRIM_MEDIA_CURRENT_URL = 2;
        public static readonly LSLInteger PRIM_MEDIA_HOME_URL = 3;
        public static readonly LSLInteger PRIM_MEDIA_AUTO_LOOP = 4;
        public static readonly LSLInteger PRIM_MEDIA_AUTO_PLAY = 5;
        public static readonly LSLInteger PRIM_MEDIA_AUTO_SCALE = 6;
        public static readonly LSLInteger PRIM_MEDIA_AUTO_ZOOM = 7;
        public static readonly LSLInteger PRIM_MEDIA_FIRST_CLICK_INTERACT = 8;
        public static readonly LSLInteger PRIM_MEDIA_WIDTH_PIXELS = 9;
        public static readonly LSLInteger PRIM_MEDIA_HEIGHT_PIXELS = 10;
        public static readonly LSLInteger PRIM_MEDIA_WHITELIST_ENABLE = 11;
        public static readonly LSLInteger PRIM_MEDIA_WHITELIST = 12;
        public static readonly LSLInteger PRIM_MEDIA_PERMS_INTERACT = 13;
        public static readonly LSLInteger PRIM_MEDIA_PERMS_CONTROL = 14;

        public static readonly LSLInteger PRIM_MEDIA_CONTROLS_STANDARD = 0;
        public static readonly LSLInteger PRIM_MEDIA_CONTROLS_MINI = 1;

        public static readonly LSLInteger PRIM_MEDIA_PERM_NONE = 0;
        public static readonly LSLInteger PRIM_MEDIA_PERM_OWNER = 1;
        public static readonly LSLInteger PRIM_MEDIA_PERM_GROUP = 2;
        public static readonly LSLInteger PRIM_MEDIA_PERM_ANYONE = 4;

        // extra constants for llSetPrimMediaParams
        public static readonly LSLInteger LSL_STATUS_OK = new LSLInteger(0);
        public static readonly LSLInteger LSL_STATUS_MALFORMED_PARAMS = new LSLInteger(1000);
        public static readonly LSLInteger LSL_STATUS_TYPE_MISMATCH = new LSLInteger(1001);
        public static readonly LSLInteger LSL_STATUS_BOUNDS_ERROR = new LSLInteger(1002);
        public static readonly LSLInteger LSL_STATUS_NOT_FOUND = new LSLInteger(1003);
        public static readonly LSLInteger LSL_STATUS_NOT_SUPPORTED = new LSLInteger(1004);
        public static readonly LSLInteger LSL_STATUS_INTERNAL_ERROR = new LSLInteger(1999);
        public static readonly LSLInteger LSL_STATUS_WHITELIST_FAILED = new LSLInteger(2001);

        public static readonly LSLInteger PARCEL_FLAG_ALLOW_FLY = 0x1;                           // parcel allows flying
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_SCRIPTS = 0x2;                       // parcel allows outside scripts
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_LANDMARK = 0x8;                      // parcel allows landmarks to be created
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_TERRAFORM = 0x10;                    // parcel allows anyone to terraform the land
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_DAMAGE = 0x20;                       // parcel allows damage
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_CREATE_OBJECTS = 0x40;               // parcel allows anyone to create objects
        public static readonly LSLInteger PARCEL_FLAG_USE_ACCESS_GROUP = 0x100;                  // parcel limits access to a group
        public static readonly LSLInteger PARCEL_FLAG_USE_ACCESS_LIST = 0x200;                   // parcel limits access to a list of residents
        public static readonly LSLInteger PARCEL_FLAG_USE_BAN_LIST = 0x400;                      // parcel uses a ban list, including restricting access based on payment info
        public static readonly LSLInteger PARCEL_FLAG_USE_LAND_PASS_LIST = 0x800;                // parcel allows passes to be purchased
        public static readonly LSLInteger PARCEL_FLAG_LOCAL_SOUND_ONLY = 0x8000;                 // parcel restricts spatialized sound to the parcel
        public static readonly LSLInteger PARCEL_FLAG_RESTRICT_PUSHOBJECT = 0x200000;            // parcel restricts llPushObject
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_GROUP_SCRIPTS = 0x2000000;           // parcel allows scripts owned by group
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_CREATE_GROUP_OBJECTS = 0x4000000;    // parcel allows group object creation
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_ALL_OBJECT_ENTRY = 0x8000000;        // parcel allows objects owned by any user to enter
        public static readonly LSLInteger PARCEL_FLAG_ALLOW_GROUP_OBJECT_ENTRY = 0x10000000;     // parcel allows with the same group to enter

        public static readonly LSLInteger REGION_FLAG_ALLOW_DAMAGE = 0x1;                        // region is entirely damage enabled
        public static readonly LSLInteger REGION_FLAG_FIXED_SUN = 0x10;                          // region has a fixed sun position
        public static readonly LSLInteger REGION_FLAG_BLOCK_TERRAFORM = 0x40;                    // region terraforming disabled
        public static readonly LSLInteger REGION_FLAG_SANDBOX = 0x100;                           // region is a sandbox
        public static readonly LSLInteger REGION_FLAG_DISABLE_COLLISIONS = 0x1000;               // region has disabled collisions
        public static readonly LSLInteger REGION_FLAG_DISABLE_PHYSICS = 0x4000;                  // region has disabled physics
        public static readonly LSLInteger REGION_FLAG_BLOCK_FLY = 0x80000;                       // region blocks flying
        public static readonly LSLInteger REGION_FLAG_ALLOW_DIRECT_TELEPORT = 0x100000;          // region allows direct teleports
        public static readonly LSLInteger REGION_FLAG_RESTRICT_PUSHOBJECT = 0x400000;            // region restricts llPushObject

        public static readonly LSLInteger PAY_HIDE = new LSLInteger(-1);
        public static readonly LSLInteger PAY_DEFAULT = new LSLInteger(-2);

        public static readonly LSLInteger PAYMENT_INFO_ON_FILE = 0x1;
        public static readonly LSLInteger PAYMENT_INFO_USED = 0x2;

        public static readonly string NULL_KEY = "00000000-0000-0000-0000-000000000000";
        public static readonly string EOF = "\n\n\n";
        public static readonly double PI = 3.1415926535897932384626433832795;
        public static readonly double TWO_PI = 6.283185307179586476925286766559;
        public static readonly double PI_BY_TWO = 1.5707963267948966192313216916398;
        public static readonly double DEG_TO_RAD = 0.01745329238f;
        public static readonly double RAD_TO_DEG = 57.29578f;
        public static readonly double SQRT2 = 1.4142135623730950488016887242097;
        public static readonly LSLInteger STRING_TRIM_HEAD = 1;
        public static readonly LSLInteger STRING_TRIM_TAIL = 2;
        public static readonly LSLInteger STRING_TRIM = 3;
        public static readonly LSLInteger LIST_STAT_RANGE = 0;
        public static readonly LSLInteger LIST_STAT_MIN = 1;
        public static readonly LSLInteger LIST_STAT_MAX = 2;
        public static readonly LSLInteger LIST_STAT_MEAN = 3;
        public static readonly LSLInteger LIST_STAT_MEDIAN = 4;
        public static readonly LSLInteger LIST_STAT_STD_DEV = 5;
        public static readonly LSLInteger LIST_STAT_SUM = 6;
        public static readonly LSLInteger LIST_STAT_SUM_SQUARES = 7;
        public static readonly LSLInteger LIST_STAT_NUM_COUNT = 8;
        public static readonly LSLInteger LIST_STAT_GEOMETRIC_MEAN = 9;
        public static readonly LSLInteger LIST_STAT_HARMONIC_MEAN = 100;

        //ParcelPrim Categories
        public static readonly LSLInteger PARCEL_COUNT_TOTAL = 0;
        public static readonly LSLInteger PARCEL_COUNT_OWNER = 1;
        public static readonly LSLInteger PARCEL_COUNT_GROUP = 2;
        public static readonly LSLInteger PARCEL_COUNT_OTHER = 3;
        public static readonly LSLInteger PARCEL_COUNT_SELECTED = 4;
        public static readonly LSLInteger PARCEL_COUNT_TEMP = 5;

        public static readonly LSLInteger DEBUG_CHANNEL = 0x7FFFFFFF;
        public static readonly LSLInteger PUBLIC_CHANNEL = 0x00000000;

        public static readonly LSLInteger OBJECT_UNKNOWN_DETAIL = -1;
        public static readonly LSLInteger OBJECT_NAME = 1;
        public static readonly LSLInteger OBJECT_DESC = 2;
        public static readonly LSLInteger OBJECT_POS = 3;
        public static readonly LSLInteger OBJECT_ROT = 4;
        public static readonly LSLInteger OBJECT_VELOCITY = 5;
        public static readonly LSLInteger OBJECT_OWNER = 6;
        public static readonly LSLInteger OBJECT_GROUP = 7;
        public static readonly LSLInteger OBJECT_CREATOR = 8;
        public static readonly LSLInteger OBJECT_RUNNING_SCRIPT_COUNT = 9;
        public static readonly LSLInteger OBJECT_TOTAL_SCRIPT_COUNT = 10;
        public static readonly LSLInteger OBJECT_SCRIPT_MEMORY = 11;

        public static readonly vector ZERO_VECTOR = new vector(0.0, 0.0, 0.0);
        public static readonly rotation ZERO_ROTATION = new rotation(0.0, 0.0, 0.0, 1.0);

        // constants for llSetCameraParams
        public static readonly LSLInteger CAMERA_PITCH = 0;
        public static readonly LSLInteger CAMERA_FOCUS_OFFSET = 1;
        public static readonly LSLInteger CAMERA_FOCUS_OFFSET_X = 2;
        public static readonly LSLInteger CAMERA_FOCUS_OFFSET_Y = 3;
        public static readonly LSLInteger CAMERA_FOCUS_OFFSET_Z = 4;
        public static readonly LSLInteger CAMERA_POSITION_LAG = 5;
        public static readonly LSLInteger CAMERA_FOCUS_LAG = 6;
        public static readonly LSLInteger CAMERA_DISTANCE = 7;
        public static readonly LSLInteger CAMERA_BEHINDNESS_ANGLE = 8;
        public static readonly LSLInteger CAMERA_BEHINDNESS_LAG = 9;
        public static readonly LSLInteger CAMERA_POSITION_THRESHOLD = 10;
        public static readonly LSLInteger CAMERA_FOCUS_THRESHOLD = 11;
        public static readonly LSLInteger CAMERA_ACTIVE = 12;
        public static readonly LSLInteger CAMERA_POSITION = 13;
        public static readonly LSLInteger CAMERA_POSITION_X = 14;
        public static readonly LSLInteger CAMERA_POSITION_Y = 15;
        public static readonly LSLInteger CAMERA_POSITION_Z = 16;
        public static readonly LSLInteger CAMERA_FOCUS = 17;
        public static readonly LSLInteger CAMERA_FOCUS_X = 18;
        public static readonly LSLInteger CAMERA_FOCUS_Y = 19;
        public static readonly LSLInteger CAMERA_FOCUS_Z = 20;
        public static readonly LSLInteger CAMERA_POSITION_LOCKED = 21;
        public static readonly LSLInteger CAMERA_FOCUS_LOCKED = 22;

        // constants for llGetParcelDetails
        public static readonly LSLInteger PARCEL_DETAILS_NAME = 0;
        public static readonly LSLInteger PARCEL_DETAILS_DESC = 1;
        public static readonly LSLInteger PARCEL_DETAILS_OWNER = 2;
        public static readonly LSLInteger PARCEL_DETAILS_GROUP = 3;
        public static readonly LSLInteger PARCEL_DETAILS_AREA = 4;
        public static readonly LSLInteger PARCEL_DETAILS_ID = 5;

        // constants for llSetClickAction
        public static readonly LSLInteger CLICK_ACTION_NONE = 0;
        public static readonly LSLInteger CLICK_ACTION_TOUCH = 0;
        public static readonly LSLInteger CLICK_ACTION_SIT = 1;
        public static readonly LSLInteger CLICK_ACTION_BUY = 2;
        public static readonly LSLInteger CLICK_ACTION_PAY = 3;
        public static readonly LSLInteger CLICK_ACTION_OPEN = 4;
        public static readonly LSLInteger CLICK_ACTION_PLAY = 5;
        public static readonly LSLInteger CLICK_ACTION_OPEN_MEDIA = 6;

        // constants for the llDetectedTouch* functions
        public static readonly LSLInteger TOUCH_INVALID_FACE = -1;
        public static readonly vector TOUCH_INVALID_TEXCOORD = new vector(-1.0, -1.0, 0.0);
        public static readonly vector TOUCH_INVALID_VECTOR = ZERO_VECTOR;

        // Constants for default textures
        public const string TEXTURE_BLANK = "5748decc-f629-461c-9a36-a35a221fe21f";
        public const string TEXTURE_DEFAULT = "89556747-24cb-43ed-920b-47caed15465f";
        public const string TEXTURE_PLYWOOD = "89556747-24cb-43ed-920b-47caed15465f";
        public const string TEXTURE_TRANSPARENT = "8dcd4a48-2d37-4909-9f78-f7a9eb4ef903";
        public const string TEXTURE_MEDIA = "8b5fec65-8d8d-9dc5-cda8-8fdf2716e361";
        
        // Constants for osGetRegionStats
        public static readonly LSLInteger STATS_TIME_DILATION = 0;
        public static readonly LSLInteger STATS_SIM_FPS = 1;
        public static readonly LSLInteger STATS_PHYSICS_FPS = 2;
        public static readonly LSLInteger STATS_AGENT_UPDATES = 3;
        public static readonly LSLInteger STATS_ROOT_AGENTS = 4;
        public static readonly LSLInteger STATS_CHILD_AGENTS = 5;
        public static readonly LSLInteger STATS_TOTAL_PRIMS = 6;
        public static readonly LSLInteger STATS_ACTIVE_PRIMS = 7;
        public static readonly LSLInteger STATS_FRAME_MS = 8;
        public static readonly LSLInteger STATS_NET_MS = 9;
        public static readonly LSLInteger STATS_PHYSICS_MS = 10;
        public static readonly LSLInteger STATS_IMAGE_MS = 11;
        public static readonly LSLInteger STATS_OTHER_MS = 12;
        public static readonly LSLInteger STATS_IN_PACKETS_PER_SECOND = 13;
        public static readonly LSLInteger STATS_OUT_PACKETS_PER_SECOND = 14;
        public static readonly LSLInteger STATS_UNACKED_BYTES = 15;
        public static readonly LSLInteger STATS_AGENT_MS = 16;
        public static readonly LSLInteger STATS_PENDING_DOWNLOADS = 17;
        public static readonly LSLInteger STATS_PENDING_UPLOADS = 18;
        public static readonly LSLInteger STATS_ACTIVE_SCRIPTS = 19;
        public static readonly LSLInteger STATS_SCRIPT_LPS = 20;

        public const string URL_REQUEST_GRANTED = "URL_REQUEST_GRANTED";
        public const string URL_REQUEST_DENIED = "URL_REQUEST_DENIED";

        public static readonly LSLInteger PASS_IF_NOT_HANDLED = 0;
        public static readonly LSLInteger PASS_ALWAYS = 1;
        public static readonly LSLInteger PASS_NEVER = 2;

        public static readonly LSLInteger RC_REJECT_TYPES = 2;
        public static readonly LSLInteger RC_DATA_FLAGS = 4;
        public static readonly LSLInteger RC_MAX_HITS = 8;
        public static readonly LSLInteger RC_DETECT_PHANTOM = 16;

        public static readonly LSLInteger RC_REJECT_AGENTS = 2;
        public static readonly LSLInteger RC_REJECT_PHYSICAL = 4;
        public static readonly LSLInteger RC_REJECT_NONPHYSICAL = 8;
        public static readonly LSLInteger RC_REJECT_LAND = 16;

        public static readonly LSLInteger RC_GET_NORMAL = 2;
        public static readonly LSLInteger RC_GET_ROOT_KEY = 4;
        public static readonly LSLInteger RC_GET_LINK_NUM = 8;

        public static readonly LSLInteger RCERR_CAST_TIME_EXCEEDED = 1;

        public static readonly LSLInteger PROFILE_NONE = 0;
        public static readonly LSLInteger PROFILE_SCRIPT_MEMORY = 1;

        public static readonly LSLInteger BOT_FOLLOW_FLAG_NONE = 0;
        public static readonly LSLInteger BOT_FOLLOW_FLAG_INDEFINITELY = 1;

        public static readonly LSLInteger BOT_FOLLOW_WALK = 0;
        public static readonly LSLInteger BOT_FOLLOW_FLY = 1;
        public static readonly LSLInteger BOT_FOLLOW_TELEPORT = 2;
        public static readonly LSLInteger BOT_FOLLOW_WAIT = 3;
    }
}
