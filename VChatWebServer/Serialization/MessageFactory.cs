using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VChatWebServer.Models.Messages;

namespace VChatWebServer.Serialization
{
    public enum MessageKind
    {
        Unknown,
        Ping,
        Pong,
        System,
        Gift,
        NewVip,
        SuperChat,
        Danmaku,
        UserMessage,
        CharMessage,
        OperaMessage
    }

    public static class MessageFactory
    {
        public static MessageKind GetKind(MessageBase message)
        {
            if (message is PingMessage) return MessageKind.Ping;
            if (message is PongMessage) return MessageKind.Pong;
            if (message is SystemMessage) return MessageKind.System;
            if (message is GiftMessage) return MessageKind.Gift;
            if (message is NewVipMessage) return MessageKind.NewVip;
            if (message is SuperChatMessage) return MessageKind.SuperChat;
            if (message is DanmakuMessage) return MessageKind.Danmaku;
            if (message is UserMessage) return MessageKind.UserMessage;
            if (message is CharMessage) return MessageKind.CharMessage;
            if (message is OperaMessage) return MessageKind.OperaMessage;
            return MessageKind.Unknown;
        }

        public static string GetDefaultAction(MessageKind kind)
        {
            switch (kind)
            {
                case MessageKind.Ping:
                    return "ping";
                case MessageKind.Pong:
                    return "pong";
                case MessageKind.System:
                    return "system";
                case MessageKind.Gift:
                    return "gift";
                case MessageKind.NewVip:
                    return "new-vip";
                case MessageKind.SuperChat:
                    return "superchat";
                case MessageKind.Danmaku:
                    return "danmaku";
                case MessageKind.UserMessage:
                    return "user-message";
                case MessageKind.CharMessage:
                    return "char-message";
                case MessageKind.OperaMessage:
                    return "opera-message";
                default:
                    return "unknown";
            }
        }

        public static MessageKind GetKind(string? action)
        {
            if (string.IsNullOrWhiteSpace(action)) return MessageKind.Unknown;
            var a = action.ToLowerInvariant();
            if (a == "ping") return MessageKind.Ping;
            if (a == "pong") return MessageKind.Pong;
            if (a == "system") return MessageKind.System;
            if (a.EndsWith("-gift") || a == "gift") return MessageKind.Gift;
            if (a.EndsWith("-new-vip") || a == "new-vip") return MessageKind.NewVip;
            if (a.EndsWith("-superchat") || a == "superchat") return MessageKind.SuperChat;
            if (a.EndsWith("-danmaku") || a == "danmaku") return MessageKind.Danmaku;
            if (a.EndsWith("-user-message") || a == "user-message") return MessageKind.UserMessage;
            if (a.EndsWith("-char-message") || a == "char-message") return MessageKind.CharMessage;
            if (a.EndsWith("-opera-message") || a == "opera-message") return MessageKind.OperaMessage;
            return MessageKind.Unknown;
        }

        public static MessageBase? Deserialize(string json, JsonSerializerOptions? options = null)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var action = root.TryGetProperty("action", out var actionProp) ? actionProp.GetString() : null;
            var kind = GetKind(action);
            switch (kind)
            {
                case MessageKind.Ping:
                    return JsonSerializer.Deserialize<PingMessage>(json, options);
                case MessageKind.Pong:
                    return JsonSerializer.Deserialize<PongMessage>(json, options);
                case MessageKind.System:
                    return JsonSerializer.Deserialize<SystemMessage>(json, options);
                case MessageKind.Gift:
                    return JsonSerializer.Deserialize<GiftMessage>(json, options);
                case MessageKind.NewVip:
                    return JsonSerializer.Deserialize<NewVipMessage>(json, options);
                case MessageKind.SuperChat:
                    return JsonSerializer.Deserialize<SuperChatMessage>(json, options);
                case MessageKind.Danmaku:
                    return JsonSerializer.Deserialize<DanmakuMessage>(json, options);
                case MessageKind.UserMessage:
                    return JsonSerializer.Deserialize<UserMessage>(json, options);
                case MessageKind.CharMessage:
                    return JsonSerializer.Deserialize<CharMessage>(json, options);
                case MessageKind.OperaMessage:
                    return JsonSerializer.Deserialize<OperaMessage>(json, options);
                default:
                    return null;
            }
        }

        public static string Serialize(MessageBase message, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(message.Action))
            {
                var kind = GetKind(message);
                message.Action = GetDefaultAction(kind);
            }
            return JsonSerializer.Serialize(message, options);
        }

        public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null) where T : MessageBase
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }

    public class MessageBaseJsonConverter : JsonConverter<MessageBase>
    {
        public override MessageBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            var action = root.TryGetProperty("action", out var actionProp) ? actionProp.GetString() : null;
            var kind = MessageFactory.GetKind(action);
            switch (kind)
            {
                case MessageKind.Ping:
                    return JsonSerializer.Deserialize<PingMessage>(root.GetRawText(), options);
                case MessageKind.Pong:
                    return JsonSerializer.Deserialize<PongMessage>(root.GetRawText(), options);
                case MessageKind.System:
                    return JsonSerializer.Deserialize<SystemMessage>(root.GetRawText(), options);
                case MessageKind.Gift:
                    return JsonSerializer.Deserialize<GiftMessage>(root.GetRawText(), options);
                case MessageKind.NewVip:
                    return JsonSerializer.Deserialize<NewVipMessage>(root.GetRawText(), options);
                case MessageKind.SuperChat:
                    return JsonSerializer.Deserialize<SuperChatMessage>(root.GetRawText(), options);
                case MessageKind.Danmaku:
                    return JsonSerializer.Deserialize<DanmakuMessage>(root.GetRawText(), options);
                case MessageKind.UserMessage:
                    return JsonSerializer.Deserialize<UserMessage>(root.GetRawText(), options);
                case MessageKind.CharMessage:
                    return JsonSerializer.Deserialize<CharMessage>(root.GetRawText(), options);
                case MessageKind.OperaMessage:
                    return JsonSerializer.Deserialize<OperaMessage>(root.GetRawText(), options);
                default:
                    return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, MessageBase value, JsonSerializerOptions options)
        {
            if (string.IsNullOrWhiteSpace(value.Action))
            {
                var kind = MessageFactory.GetKind(value);
                value.Action = MessageFactory.GetDefaultAction(kind);
            }
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}

