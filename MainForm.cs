using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SyncDataTool.BLL;
using SyncDataTool.Entity;
using System.Linq;

namespace SyncDataTool
{
    public partial class MainForm : Form
    {
        #region 私有变量
        private static XmlNodeList xmlNodes = null;
        private static List<string> lstProductIds = new List<string>();
        private static List<Dictionary<string, string>> lstDicWebsites = new List<Dictionary<string, string>>();
        private static string strCurrentCountry = string.Empty;
        private static string strSourceWebsite = string.Empty;
        private static string strCurrentId = string.Empty;
        private static DateTime dtStartTime = DateTime.MinValue;
        private Thread thSyncData = null;
        private static string strPwdHashCode = string.Empty;
        #endregion

        #region 构造函数
        public MainForm()
        {
            InitializeComponent();           
        }
        #endregion

        #region 窗体加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadWebsiteToControls();
            this.Text = string.Format("{0} - {1}", this.Text, Common.AssemblyFileVersion());
        }
        #endregion

        #region 通知UI更新状态
        /// <summary>
        /// 通知状态的委托
        /// </summary>
        /// <param name="obj"></param>
        private delegate void NotifyStatusDelegate(object[] obj);

        /// <summary>
        /// 通知类型
        /// </summary>
        private enum NotifyType
        {
            /// <summary>
            /// 开始
            /// </summary>
            Start = 1,

            /// <summary>
            /// 处理状态
            /// </summary>
            ProcessStatus = 2,

            /// <summary>
            /// 成功
            /// </summary>
            AllSuccessfully = 3,

            /// <summary>
            /// 失败
            /// </summary>
            AllFailed = 4,

            /// <summary>
            /// 添加项
            /// </summary>
            AddItems = 5,

            /// <summary>
            /// 同步失败
            /// </summary>
            SyncFailed = 6,

            /// <summary>
            /// 同步成功
            /// </summary>
            SyncSuccess = 7,

            /// <summary>
            /// 结束
            /// </summary>
            End = 8
        }

        #region Invoke通知状态
        /// <summary>
        /// 通知状态
        /// </summary>
        /// <param name="obj"></param>        
        private void NotifyStatus(object[] obj)
        {
            Invoke(new NotifyStatusDelegate(UpdateStatus), new object[] { obj });
        }
        #endregion

        private void DisableControls(bool disable)
        {
            if (true == disable)
            {
                btn_Start.Enabled = false;
                comboBox_Websites.Enabled = false;
                textBox_Id.ReadOnly = true;
                btn_Add.Enabled = false;
                pictureBox_Delete.Enabled = false;
                pictureBox_Clear.Enabled = false;
                listBox_Product_Ids.Enabled = false;
                checkedListBox_Websites.Enabled = false;
                toolStripStatusLabel_gif.Visible = true;
            }
            else
            {
                btn_Start.Enabled = true;
                comboBox_Websites.Enabled = true;
                textBox_Id.ReadOnly = false;
                btn_Add.Enabled = true;
                pictureBox_Delete.Enabled = true;
                pictureBox_Clear.Enabled = true;
                listBox_Product_Ids.Enabled = true;
                checkedListBox_Websites.Enabled = true;
                toolStripStatusLabel_gif.Visible = false;
            }
        }

        #region 更新状态
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateStatus(object[] obj)
        {
            string id = string.Empty;
            int index = 0;
            int indexColumn = 0;
            Exception ex = null;
            #region switch body
            switch ((NotifyType)obj[0])
            {
                case NotifyType.Start:
                    this.listView_Results.Items.Clear();
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    DisableControls(true);
                    break;
                case NotifyType.ProcessStatus:
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    break;
                case NotifyType.SyncFailed:
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    indexColumn = Convert.ToInt32(obj[2]);
                    id = obj[3].ToString();
                    ex = (Exception)obj[4];
                    index = lstProductIds.IndexOf(id);
                    listView_Results.Items[index].SubItems[indexColumn].Text = "Failed";
                    listView_Results.Items[index].SubItems[indexColumn].ForeColor = Color.Red;
                    Common.WriteLog(string.Format("Sync data failed. [ID:{0}]\r\nMessage:{1}\r\nDetails:{2}", id, ex.Message, ex.StackTrace));
                    break;
                case NotifyType.SyncSuccess:
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    indexColumn = Convert.ToInt32(obj[2]);
                    id = obj[3].ToString();
                    index = lstProductIds.IndexOf(id);
                    if (Convert.ToBoolean(obj[4]))
                    {
                        listView_Results.Items[index].SubItems[indexColumn].Text = "Successfully";
                    }
                    else
                    {
                        listView_Results.Items[index].SubItems[indexColumn].Text = "Failed";
                    }
                    listView_Results.Items[index].SubItems[indexColumn].ForeColor = Color.Red;
                    break;
                case NotifyType.AllSuccessfully:
                    TimeSpan span = DateTime.Now - dtStartTime;
                    double totalSeconds = span.TotalSeconds; //耗时
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    DisableControls(false);
                    MessageBox.Show(string.Format("{0}\nTime-consuming：{1}s\nPath：{2}", obj[1], totalSeconds, obj[2]), "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case NotifyType.AddItems:
                    ListViewItem item = new ListViewItem(obj[1].ToString());
                    item.UseItemStyleForSubItems = false;
                    item.SubItems[0].ForeColor = Color.Blue;
                    for (int i = 0; i < listView_Results.Columns.Count - 1; i++)
                    {
                        item.SubItems.Add("Pending");//, Color.Blue, item.BackColor, item.Font);
                    }
                    this.listView_Results.Items.Add(item);
                    break;
                case NotifyType.AllFailed:
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    id = obj[2].ToString();
                    ex = (Exception)obj[3];
                    index = lstProductIds.IndexOf(id);
                    foreach (ListViewItem.ListViewSubItem subItem in listView_Results.Items[index].SubItems)
                    {
                        if (listView_Results.Items[index].SubItems.IndexOf(subItem) > 0)
                        {
                            subItem.Text = "Failed";
                            subItem.ForeColor = Color.Red;
                        }
                    }
                    Common.WriteLog(string.Format("Download data failed. [ID:{0}]\r\nMessage:{1}\r\nDetails:{2}", id, ex.Message, ex.StackTrace));
                    break;
                case NotifyType.End:
                    DisableControls(false);
                    this.toolStripStatusLabel_Status.Text = obj[1].ToString();
                    break;
            }
            Application.DoEvents();
            #endregion
        }
        #endregion

        #endregion

        #region 加载站点数据到控件
        private void LoadWebsiteToControls()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "settings.xml")));
            strPwdHashCode = xdoc["settings"]["pwd"].InnerText;
            xmlNodes = xdoc["settings"]["website"].ChildNodes;
            for (int i = 0; i < xmlNodes.Count; i++)
            {
                this.comboBox_Websites.Items.Add(xmlNodes[i].Attributes["country"].Value.Trim());
                if (i > 0)
                {
                    this.checkedListBox_Websites.Items.Add(xmlNodes[i].Attributes["country"].Value.Trim(), false);
                }
            }
            this.comboBox_Websites.SelectedIndex = 0;
        }
        #endregion

        #region 增加Product ID按钮
        private void btn_Add_Click(object sender, EventArgs e)
        {
            string id = textBox_Id.Text.Trim();
            if (string.IsNullOrEmpty(id) == false)
            {
                string[] lstIds = null;
                if (id.Contains(","))
                {
                    lstIds = id.Split(',');
                }
                if (lstIds != null)
                {
                    foreach (string str in lstIds)
                    {
                        if (string.IsNullOrEmpty(str) == false)
                        {
                            if (lstProductIds.Contains(str) == false)
                            {
                                this.listBox_Product_Ids.Items.Add(str);
                                this.listBox_Product_Ids.SelectedIndex = listBox_Product_Ids.Items.Count - 1;
                                lstProductIds.Add(str);
                            }
                            else
                            {
                                this.listBox_Product_Ids.SelectedItem = str;
                            }
                        }
                    }
                }
                else
                {
                    if (lstProductIds.Contains(id) == false)
                    {
                        this.listBox_Product_Ids.Items.Add(id);
                        this.listBox_Product_Ids.SelectedIndex = listBox_Product_Ids.Items.Count - 1;
                        lstProductIds.Add(id);
                    }
                    else
                    {
                        this.listBox_Product_Ids.SelectedItem = id;
                    }
                }
            }
            textBox_Id.Text = string.Empty;
            textBox_Id.Focus();
            CheckAndChangeStartButton();
        }
        #endregion

        #region 选择站点项的监听
        private void checkedListBox_Websites_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string strColumnHeader = checkedListBox_Websites.Items[e.Index].ToString();
            if (e.CurrentValue == CheckState.Unchecked)
            {
                this.listView_Results.Columns.Add(strColumnHeader, 110);
                //Refresh
                this.listView_Results.Columns.Add(string.Empty);
                this.listView_Results.Columns.RemoveAt(this.listView_Results.Columns.Count - 1);

                bool blEnable = true;
                if (comboBox_Websites.Items.Count == 0)
                {
                    blEnable = false;
                }

                if (listBox_Product_Ids.Items.Count == 0)
                {
                    blEnable = false;
                }
                btn_Start.Enabled = blEnable;
            }
            else
            {
                foreach (ColumnHeader columnheader in listView_Results.Columns)
                {
                    if (columnheader.Text == strColumnHeader)
                    {
                        this.listView_Results.Columns.Remove(columnheader);
                        bool blEnable = true;
                        if (comboBox_Websites.Items.Count == 0)
                        {
                            blEnable = false;
                        }
                        if (listBox_Product_Ids.Items.Count == 0)
                        {
                            blEnable = false;
                        }
                        if (checkedListBox_Websites.CheckedItems.Count <= 1)
                        {
                            blEnable = false;
                        }
                        btn_Start.Enabled = blEnable;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Product ID 列表选中项监听
        private void listBox_Product_Ids_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_Product_Ids.Items.Count > 0)
            {
                pictureBox_Delete.Show();
                pictureBox_Clear.Show();
            }
            else
            {
                pictureBox_Delete.Hide();
                pictureBox_Clear.Hide();
            }
        }
        #endregion

        #region 输入Product ID的文本框按键监听
        private void textBox_Id_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Add_Click(null, null);
            }
        }
        #endregion

        #region 移除Product ID
        private void pictureBox_Delete_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(string.Format("Delete Product ID: {0} ?", listBox_Product_Ids.SelectedItem.ToString()), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                lstProductIds.Remove(listBox_Product_Ids.SelectedItem.ToString());
                this.listBox_Product_Ids.Items.Remove(listBox_Product_Ids.SelectedItem);
                this.listBox_Product_Ids.SelectedIndex = listBox_Product_Ids.Items.Count - 1;
            }
            CheckAndChangeStartButton();
        }
        #endregion

        #region 清除所有Product ID
        private void pictureBox_Clear_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Clear List?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                this.listBox_Product_Ids.Items.Clear();
                lstProductIds.Clear();
                pictureBox_Delete.Hide();
                pictureBox_Clear.Hide();
            }
            CheckAndChangeStartButton();
        }
        #endregion

        #region 检查并改变开始按钮
        private void CheckAndChangeStartButton()
        {
            bool blEnable = true;
            if (comboBox_Websites.Items.Count == 0)
            {
                blEnable = false;
            }

            if (listBox_Product_Ids.Items.Count == 0)
            {
                blEnable = false;
            }

            if (checkedListBox_Websites.CheckedItems.Count == 0)
            {
                blEnable = false;
            }
            btn_Start.Enabled = blEnable;
        }
        #endregion

        #region 开始按钮
        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_Password.Text))
            {
                MessageBox.Show("PLEASE INPUT PASSWORD!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_Password.Focus();
                return;
            }

            if (strPwdHashCode.ToUpper() != Common.GetMd5Hash(textBox_Password.Text).ToUpper())
            {
                MessageBox.Show("PASSWORD ERROR!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_Password.Focus();
                return;
            }

            thSyncData = new Thread(new ThreadStart(SyncData));
            thSyncData.IsBackground = true;
            thSyncData.Start();
        }
        #endregion

        #region 同步数据Logic
        private void SyncData()
        {
            NotifyStatus(new object[] { NotifyType.Start, "Starting..." });
            string strUrl = string.Empty;
            ProductEntity product = new ProductEntity();
            int failedCount = 0;
            foreach (string id in lstProductIds)
            {
                NotifyStatus(new object[] { NotifyType.AddItems, id });
                NotifyStatus(new object[] { NotifyType.ProcessStatus, string.Format("Downloading data... [ID:{0}]", id) });
                strUrl = string.Format("{0}/products/{1}.json", strSourceWebsite, id);
                product = new ProductEntity();
                try
                {
                    product = Product.GetProduct(strUrl);
                    NotifyStatus(new object[] { NotifyType.ProcessStatus, string.Format("Download successfully. [ID:{0}]", id) });
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    failedCount = failedCount + listView_Results.Columns.Count - 1;
                    NotifyStatus(new object[] { NotifyType.AllFailed, string.Format("{0} [ID:{1}]", ex.Message, id), id, ex });
                    Thread.Sleep(500);
                    continue;
                }

                ProductEntity sourceProduct = new ProductEntity();
                foreach (ColumnHeader column in listView_Results.Columns)
                {
                    sourceProduct = product.Clone() as ProductEntity;
                    if (column.Text != listView_Results.Columns[0].Text)
                    {
                        NotifyStatus(new object[] { NotifyType.ProcessStatus, string.Format("Sync to \"{0}\" ... [ID:{1}]", column.Text, id) });
                        Thread.Sleep(500);
                        strUrl = string.Format("{0}/products.json", GetUrlByColumn(column.Text));
                        try
                        {
                            //源站点的价格偏移量
                            double source_price_offset = GetPriceOffsetByColumn(strCurrentCountry);
                            //目标站点的价格偏移量
                            double target_price_offset = GetPriceOffsetByColumn(column.Text);

                            string source_country_code = GetCountryCodeByColumn(strCurrentCountry);
                            string target_country_code = GetCountryCodeByColumn(column.Text);
                            string strSourceUrl = GetUrlByCountryCode(source_country_code);

                            bool result = Product.CreateProduct(strSourceUrl, strUrl, sourceProduct, source_price_offset, target_price_offset, source_country_code, target_country_code);
                            NotifyStatus(new object[] { NotifyType.SyncSuccess, string.Format("Sync to \"{0}\" successfully. [ID:{1}]"
                                , column.Text, id), listView_Results.Columns.IndexOf(column),id ,result});
                            Thread.Sleep(500);
                        }
                        catch (Exception ex)
                        {
                            failedCount++;
                            NotifyStatus(new object[] { NotifyType.SyncFailed, string.Format("{0} [ID:{1}]", ex.Message, id), listView_Results.Columns.IndexOf(column), id, ex });
                            Thread.Sleep(500);
                            continue;
                        }
                    }
                }
            }
            NotifyStatus(new object[] { NotifyType.End, string.Format("Finished. ({0} Failed)", failedCount) });
        }
        #endregion

        #region 选择源站点
        private void comboBox_Websites_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox_Websites.Items.Clear();
            foreach (ColumnHeader header in listView_Results.Columns)
            {
                if (listView_Results.Columns.IndexOf(header) > 0)
                {
                    listView_Results.Columns.Remove(header);
                }
            }
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes["country"].Value.Trim() == comboBox_Websites.SelectedItem.ToString())
                {
                    strCurrentCountry = comboBox_Websites.SelectedItem.ToString();
                    strSourceWebsite = node.InnerText;
                }
                else
                {
                    checkedListBox_Websites.Items.Add(node.Attributes["country"].Value.Trim());
                }
            }
        }
        #endregion

        #region 根据列头获取地址
        private string GetUrlByColumn(string column)
        {
            string strWebsite = string.Empty;
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes["country"].Value.Trim() == column)
                {
                    strWebsite = node.InnerText;
                    break;
                }
            }
            return strWebsite;
        }
        #endregion

        #region 根据列头获取价格偏移量
        private double GetPriceOffsetByColumn(string column)
        {
            double price_Offset = 0;
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes["country"].Value.Trim() == column)
                {
                    price_Offset = Convert.ToDouble(node.Attributes["price_offset"].Value.Trim());
                    break;
                }
            }
            return price_Offset;
        }
        #endregion

        #region 根据列头获取国家简码
        private string GetCountryCodeByColumn(string column)
        {
            string strCode = string.Empty;
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes["country"].Value.Trim() == column)
                {
                    strCode = node.Attributes["code"].Value.Trim();
                    break;
                }
            }
            return strCode;
        }
        #endregion

        #region 根据国家简码获取URL地址
        private string GetUrlByCountryCode(string strCode)
        {
            string strUrl = string.Empty;
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes["code"].Value.Trim().ToLower() == strCode.ToLower())
                {
                    strUrl = node.InnerText.Trim();
                    break;
                }
            }
            return strUrl;
        }
        #endregion
    }
}