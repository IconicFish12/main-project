namespace main_project.Services
{
    public enum FileState
    {
        Idle, // default
        inProgress, 
        isCompleted, // task selesai
        isSaved,
        isUpdated,
        isDeleted,
        Failed
    }
}