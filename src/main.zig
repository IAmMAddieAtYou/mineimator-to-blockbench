const std = @import("std");

pub fn convert(allocator: std.mem.Allocator, text: []const u8, file_name: []const u8) !void {
    // data
    var tempo: u64 = 0;
    var length: u64 = 0;

    const bone_data = struct {
        position: ?std.json.ArrayHashMap([3]f64),
        rotation: ?std.json.ArrayHashMap([3]f64),
        scale: ?std.json.ArrayHashMap([3]f64),
    };

    var bones = std.StringArrayHashMapUnmanaged(bone_data){};

    // parse json
    const parsed = try std.json.parseFromSlice(std.json.Value, allocator, text, .{});

    var root = parsed.value;
    tempo = @intCast(root.object.get("tempo").?.integer);
    length = @intCast(root.object.get("length").?.integer);

    // get root bone
    std.debug.print("enter root bone name for {s}\n", .{file_name});
    std.debug.print("> ", .{});

    const stdin = std.io.getStdIn();
    const input = try stdin.reader().readUntilDelimiterOrEofAlloc(allocator, '\n', 1024) orelse return error.NoInput;

    for (root.object.get("keyframes").?.array.items) |keyframe| {
        const position: u64 = @intCast(keyframe.object.get("position").?.integer);

        // get bone name
        var bone_name: []const u8 = undefined;
        if (keyframe.object.get("part_name") != null) {
            bone_name = keyframe.object.get("part_name").?.string;
        } else {
            bone_name = input[0..input.len-1];
        }

        var X_POS: f64 = undefined;
        var Y_POS: f64 = undefined;
        var Z_POS: f64 = undefined;

        var X_ROT: f64 = undefined;
        var Y_ROT: f64 = undefined;
        var Z_ROT: f64 = undefined;

        var X_SCA: f64 = undefined;
        var Y_SCA: f64 = undefined;
        var Z_SCA: f64 = undefined;

        const values = keyframe.object.get("values").?.object;

        if (values.get("POS_X") != null) {
            switch (values.get("POS_X").?) {
                .integer => |i| X_POS = @floatFromInt(i),
                .float => |f| X_POS = f,
                else => {
                    std.debug.print("invalid POS_X", .{});
                },
            }
        } else {
            X_POS = 0;
        }

        if (values.get("POS_Y") != null) {
            switch (values.get("POS_Y").?) {
                .integer => |i| Y_POS = @floatFromInt(i),
                .float => |f| Y_POS = f,
                else => {
                    std.debug.print("invalid POS_Y", .{});
                },
            }
        } else {
            Y_POS = 0;
        }

        if (values.get("POS_Z") != null) {
            switch (values.get("POS_Z").?) {
                .integer => |i| Z_POS = @floatFromInt(i),
                .float => |f| Z_POS = f,
                else => {
                    std.debug.print("invalid POS_Z", .{});
                },
            }
        } else {
            Z_POS = 0;
        }

        if (values.get("ROT_X") != null) {
            switch (values.get("ROT_X").?) {
                .integer => |i| X_ROT = @floatFromInt(i),
                .float => |f| X_ROT = f,
                else => {
                    std.debug.print("invalid ROT_X", .{});
                },
            }
        } else {
            X_ROT = 0;
        }

        if (values.get("ROT_Y") != null) {
            switch (values.get("ROT_Y").?) {
                .integer => |i| Y_ROT = @floatFromInt(i),
                .float => |f| Y_ROT = f,
                else => {
                    std.debug.print("invalid ROT_Y", .{});
                },
            }
        } else {
            Y_ROT = 0;
        }

        if (values.get("ROT_Z") != null) {
            switch (values.get("ROT_Z").?) {
                .integer => |i| Z_ROT = @floatFromInt(i),
                .float => |f| Z_ROT = f,
                else => {
                    std.debug.print("invalid ROT_Z", .{});
                },
            }
        } else {
            Z_ROT = 0;
        }

        if (values.get("SCA_X") != null) {
            switch (values.get("SCA_X").?) {
                .integer => |i| X_SCALE = @floatFromInt(i),
                .float => |f| X_SCALE = f,
                else => {
                    std.debug.print("invalid SCA_X", .{});
                },
            }
        } else {
            X_SCA = 1;
        }

        if (values.get("SCA_Y") != null) {
            switch (values.get("SCA_Y").?) {
                .integer => |i| Y_SCALE = @floatFromInt(i),
                .float => |f| Y_SCALE = f,
                else => {
                    std.debug.print("invalid SCA_Y", .{});
                },
            }
        } else {
            Y_SCA = 1;
        }

        if (values.get("SCA_Z") != null) {
            switch (values.get("SCA_Z").?) {
                .integer => |i| Z_SCALE = @floatFromInt(i),
                .float => |f| Z_SCALE = f,
                else => {
                    std.debug.print("invalid SCALE_Z", .{});
                },
            }
        } else {
            Z_SCA = 1;
        }

        const pos_data: [3]f64 = .{
            X_POS,
            Z_POS,
            -Y_POS,
        };

        const rot_data: [3]f64 = .{
            X_ROT,
            -Z_ROT,
            -Y_ROT,
        };

        const scale_data: [3]f64 = .{
            X_SCA,
            Y_SCA,
            Z_SCA,
        };

        const frame_pos_string = try std.fmt.allocPrint(allocator, "{d}", .{@as(f64, @floatFromInt(position))/@as(f64, @floatFromInt(tempo))});

        var pos_map = std.StringArrayHashMapUnmanaged([3]f64){};
        var rot_map = std.StringArrayHashMapUnmanaged([3]f64){};
        var scale_map = std.StringArrayHashMapUnmanaged([3]f64){};

        if (!bones.contains(bone_name)) {
            try pos_map.putNoClobber(allocator, frame_pos_string, pos_data);
            try rot_map.putNoClobber(allocator, frame_pos_string, rot_data);
            try scale_map.putNoClobber(allocator, frame_pos_string, scale_data);

            const new_bone = bone_data {
                .position = .{ .map = pos_map },
                .rotation = .{ .map = rot_map },
                .scale = .{ .map = scale_map },
            };
            try bones.putNoClobber(allocator, bone_name, new_bone);
        } else {
            try bones.getPtr(bone_name).?.*.position.?.map.putNoClobber(allocator, frame_pos_string, pos_data);
            try bones.getPtr(bone_name).?.*.rotation.?.map.putNoClobber(allocator, frame_pos_string, rot_data);
            try bones.getPtr(bone_name).?.*.scale.?.map.putNoClobber(allocator, frame_pos_string, scale_data);
        }
    }

    const output = struct {
        format_version: []const u8,
        animations: struct {
            animation: struct {
                loop: []const u8,
                animation_length: f64,
                bones: std.json.ArrayHashMap(bone_data),
            }
        }
    };

    const new_output = output {
        .format_version = "1.8.0",
        .animations = .{
            .animation = .{
                .loop = "hold_on_last_frame",
                .animation_length = @as(f64, @floatFromInt(length))/@as(f64, @floatFromInt(tempo)),
                .bones = std.json.ArrayHashMap(bone_data){ .map = bones },
            }
        }
    };

    // generate new file name
    const file_dir_with_extension = try std.fmt.allocPrint(allocator, "output/{s}.animation.json", .{file_name[0..(file_name.len - 9)]});

    // stringify json
    var stringified_text = try std.json.stringifyAlloc(allocator, new_output, .{ .whitespace = .indent_tab, .emit_nonportable_numbers_as_strings = true });
    var rebuilt_text = std.ArrayList(u8).init(allocator);
    
    // convert scientific notation float to decimal float
    var starts = std.ArrayList(usize).init(allocator);
    var ends = std.ArrayList(usize).init(allocator);

    for (stringified_text, 0..) |c, i| {
        if (c == 'e') {
            _ = std.fmt.charToDigit(stringified_text[i-1], 10) catch {
                continue;
            };
            _ = std.fmt.charToDigit(stringified_text[i+1], 10) catch {
                continue;
            };

            var start: usize = i;
            while (stringified_text[start] != '\t' and stringified_text[start] != ' ') : (start -= 1) {}
            try starts.append(start);

            var end: usize = i;
            while (stringified_text[end] != ',' and stringified_text[end] != '\n') : (end += 1) {}
            try ends.append(end);
        }
    }

    for (starts.items, ends.items, 0..) |start, end, i| {
        const slice = stringified_text[start+1..end];
        const float = try std.fmt.parseFloat(f64, slice);
        const float_string = try std.fmt.allocPrint(allocator, "{d}", .{float});

        if (i == 0) {
            try rebuilt_text.appendSlice(stringified_text[0..starts.items[i]+1]);
        } else {
            try rebuilt_text.appendSlice(stringified_text[ends.items[i-1]..starts.items[i]+1]);
        }
        try rebuilt_text.appendSlice(float_string);

        if (i == starts.items.len-1) {
            try rebuilt_text.appendSlice(stringified_text[ends.items[i]..]);
        }
    }

    // write json to new file
    try std.fs.cwd().writeFile2(.{
        .sub_path = file_dir_with_extension,
        .data = rebuilt_text.items,
        .flags = .{}
    });
}

pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer _ = gpa.deinit();

    var arena = std.heap.ArenaAllocator.init(gpa.allocator());
    defer arena.deinit();
    const allocator = arena.allocator();

    var folder = try std.fs.cwd().openDir("input", .{ .iterate = true });
    defer folder.close();

    // iterate through files
    var iterator = folder.iterate();
    while (try iterator.next()) |entry| {
        if (entry.kind == .file) {
            const file = try folder.readFileAlloc(allocator, entry.name, 1048576);

            try convert(allocator, file, entry.name);
            _ = arena.reset(.retain_capacity);
        }
    }

    std.debug.print("completed", .{});
}