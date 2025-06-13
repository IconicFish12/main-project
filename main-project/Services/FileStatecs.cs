namespace main_project.Services
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