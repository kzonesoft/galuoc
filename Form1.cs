
using SteamTools.ViewModels;
using System.Threading;
using System.Windows.Forms;

namespace SteamTools
{
    public partial class Form1 : Form
    {
        private AutoGetSteamInfoViewModel _viewModel;
        public Form1()
        {
            InitializeComponent();
            //_viewModel = new SteamInfoViewModel(SynchronizationContext.Current);
            _viewModel = new AutoGetSteamInfoViewModel(SynchronizationContext.Current);
            _viewModel.HandleMessage = (type, mess) => 
            {
                if (type == 0)
                    label1.Invoke((MethodInvoker)(() => label1.Text = mess));
                else
                    MessageBox.Show(mess);
            };
            this.gridView.DataSource = _viewModel.BindingSource;
        }

        private void btnGet_Click(object sender, System.EventArgs e)
        {
            //_viewModel.GetById(txtGameId.Text);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            _viewModel.LoopCheck();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //_viewModel.SaveData();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
           // _viewModel.Reload();
        }
    }    
}
