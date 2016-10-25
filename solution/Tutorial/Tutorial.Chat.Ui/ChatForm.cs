using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tutorial.Chat.Ui
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class ChatForm : Form
    {
        public string FormTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatForm"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ChatForm(string name): this()
        {
            this.FormTitle = name;
            this.Text = $"Chat Application: {name}";
            this.ActiveControl = inputTextBox;
        }

        /// <summary>
        /// </summary>
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatForm"/> class.
        /// </summary>
        public ChatForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the submitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void submitButton_Click(object sender, EventArgs e)
        {

        }
    }
}
