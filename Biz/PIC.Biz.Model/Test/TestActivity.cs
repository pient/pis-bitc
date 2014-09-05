using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using PIC.Portal;

namespace PIC.Biz.Model.Test
{

    public sealed class TestActivity : NativeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Text { get; set; }
        public InArgument<TestState> FlowState { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string text = context.GetValue(this.Text);

            context.CreateBookmark("Test");
        }

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}
