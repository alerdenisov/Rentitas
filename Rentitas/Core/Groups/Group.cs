namespace Rentitas
{
    public partial class Group
    {
        /// Returns the number of entities in the group.
        public int Count => _entities.Count;

        /// Returns the matcher which was used to create this group.
        public IMatcher Matcher => _matcher;

        /// Use pool.GetGroup(matcher) to get a group of entities which match the specified matcher.
        public Group(IMatcher matcher)
        {
            _matcher = matcher;
        }
    }
}