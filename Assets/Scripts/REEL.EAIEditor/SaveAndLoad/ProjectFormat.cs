// Project File Format.
using System.Xml.Serialization;

namespace REEL.EAIEditor
{
    [System.Serializable]
    public class ProjectFormat
    {
        public string projectName = string.Empty;
        public NodeBlockArray blockArray;
        public LineBlockArray lineArray;

        // Wrapper Functions for block array.
        public void BlockAdd(NodeBlock block)
        {
            blockArray.Add(block);
        }

        public void BlockRemoveAt(int index)
        {
            blockArray.RemoveAt(index);
        }

        public void BlockRemove(NodeBlock block)
        {
            blockArray.Remove(block);
        }

        // Wrapper Functions for line array.
        public void LineAdd(LineBlock line)
        {
            lineArray.Add(line);
        }
    }
}