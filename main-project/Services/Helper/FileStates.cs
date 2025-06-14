namespace main_project.Services.Helper
{
    public enum FileState
    {
        Idle, // default
        inProgress,
        isCompleted,
        isSaved,
        isUpdated,
        isDeleted,
        Failed
    }
}