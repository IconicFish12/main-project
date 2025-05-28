namespace main_project.Config
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string AppEnv { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
        public StorageSettings StorageSettings { get; set; }
        public SecuritySettings Security { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public int MaximumPayload { get; set; }
        public string Provider { get; set; }
        public bool EnableRetryOnFailure { get; set; }
    }

    public class StorageSettings
    {
        public string StoragePath { get; set; }
        public string MetadataStorage { get; set; }
        public bool EnableAutoCleanup { get; set; }
        public int MaxRetentionDays { get; set; }
    }

    public class SecuritySettings
    {
        public bool EnableFileValidation { get; set; }
    }
}
