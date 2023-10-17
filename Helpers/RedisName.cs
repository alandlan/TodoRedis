namespace TodoRedis.Helpers
{
    public static class RedisName
    {
        public static string GetObjectKey<T>(string id)
        {
            return $"-{typeof(T).Name}:{id}";
        }
    }
}