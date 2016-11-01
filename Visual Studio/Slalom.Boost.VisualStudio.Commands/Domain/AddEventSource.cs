namespace Slalom.Boost.VisualStudio.Commands.Domain
{
    public class AddEventSource : VisualStudioCommand
    {
        public AddEventSource()
            : base(0x0506)
        {
            base.IsAsync = false;
        }

        public override bool ShouldDisplay()
        {
            //return projectItem.Project.IsDomainProject && (projectItem.IsProject || projectItem.IsFolder);
            return false;
        }

        protected override void Execute()
        {
            //using (var form = new AddItemForm("Add Event Source"))
            //{
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        form.Activate();

            //        new EventSourceEventBuilder(form.ItemName, this.ProjectItem).Build();
            //        new EventSourceBuilder(form.ItemName, this.ProjectItem).Build();
            //    }
            //}
        }
    }
}