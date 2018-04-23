namespace Event_ECS_WPF.Projects
{
    public enum ProjectType
    {
        NORMAL,
        LOVE
    }

    public static class ProjectTypeExtensions
    {
        public static Project CreateProject(this ProjectType type)
        {
            switch (type)
            {
                case ProjectType.LOVE:
                    return new LoveProject();
                default:
                    return new Project();
            }
        }
    }
}
