namespace Slalom.Boost.VisualStudio.Commands.Domain
{
    public class AddEvent : VisualStudioCommand
    {
        public AddEvent()
            : base(0x0513)
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
            //using (var form = new AddItemForm("Add Event"))
            //{
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        form.Activate();

            //        new EventBuilder(form.ItemName, form.Properties, this.ProjectItem).Build();
            //    }
            //}
        }
    }
}