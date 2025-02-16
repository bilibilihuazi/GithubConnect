using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Security.Policy;

//GithubConnect
//项目开始于2025/2/15 P.M.
/*
                   ____   _   _     _               _        ____                                         _                   
  _____   _____   / ___| (_) | |_  | |__    _   _  | |__    / ___|   ___    _ __    _ __     ___    ___  | |_   _____   _____ 
 |_____| |_____| | |  _  | | | __| | '_ \  | | | | | '_ \  | |      / _ \  | '_ \  | '_ \   / _ \  / __| | __| |_____| |_____|
 |_____| |_____| | |_| | | | | |_  | | | | | |_| | | |_) | | |___  | (_) | | | | | | | | | |  __/ | (__  | |_  |_____| |_____|
                  \____| |_|  \__| |_| |_|  \__,_| |_.__/   \____|  \___/  |_| |_| |_| |_|  \___|  \___|  \__|                
                                                                                                                              
*/
//作者:HuaZi-华子
//Github仓库:https://github.com/bilibilihuazi/GithubConnect
namespace GithubConnect
{
    public partial class Main_Window: Form
    {
        //变量========================================================================================
        public static string Title = "GithubConnect";//窗口标题
        public static string Version = "Release1.0.0.0";//版本
        public static string RunPath = Directory.GetCurrentDirectory();//运行路径
        public static string ConfigPath = RunPath + "\\config.ini";//配置文件路径
        public static string HostsPath = "C:\\Windows\\System32\\drivers\\etc\\hosts";
        //函数========================================================================================

        //写配置项
        public void WriteConfig(string filePath, string key, string value)
        {
            // 检查文件是否存在，如果不存在则创建
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            // 读取所有行
            var lines = File.ReadAllLines(filePath);
            var newLines = new List<string>();

            bool keyExists = false;
            foreach (var line in lines)
            {
                // 如果键存在，则更新值
                if (line.StartsWith(key + "="))
                {
                    newLines.Add(key + "=" + value);
                    keyExists = true;
                }
                else
                {
                    newLines.Add(line);
                }
            }

            // 如果键不存在，则添加新键值对
            if (!keyExists)
            {
                newLines.Add(key + "=" + value);
            }

            // 写回文件
            File.WriteAllLines(filePath, newLines);
        }

        //读配置项
        public string ReadConfig(string filePath, string key)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("配置文件未找到", filePath);
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (line.StartsWith(key + "="))
                {
                    return line.Substring(key.Length + 1);
                }
            }

            throw new KeyNotFoundException($"键 {key} 未找到");
        }

        //HTTP读取文件(同步)
        public static string HttpReadFile(string url)
        {
            try
            {
                // 设置安全协议类型（支持TLS 1.2/1.1/1.0）
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 |
                    SecurityProtocolType.Tls11 |
                    SecurityProtocolType.Tls;

                // 创建带自定义验证的HttpClient
                using (var handler = new HttpClientHandler())
                using (var client = new HttpClient(handler))
                {
                    // 忽略SSL证书验证
                    handler.ServerCertificateCustomValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => true;

                    // 设置超时时间（10秒）
                    client.Timeout = TimeSpan.FromSeconds(10);

                    // 添加浏览器User-Agent
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                        "(KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                    // 发送GET请求
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();

                    // 读取字节内容
                    var bytes = response.Content.ReadAsByteArrayAsync().Result;

                    // 检测编码
                    var encoding = DetectEncoding(response, bytes);

                    // 转换为字符串
                    return encoding.GetString(bytes);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        //HTTPS读文件(检测编码)
        private static Encoding DetectEncoding(HttpResponseMessage response, byte[] bytes)
        {
            try
            {
                // 从Content-Type头获取编码
                var contentType = response.Content.Headers.ContentType;
                if (contentType?.CharSet != null)
                {
                    return Encoding.GetEncoding(contentType.CharSet);
                }
            }
            catch
            {
                // 忽略编码解析错误
            }

            // 尝试通过BOM检测编码
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return Encoding.UTF8;
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                return Encoding.BigEndianUnicode;
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return Encoding.Unicode;

            // 默认使用UTF-8
            return Encoding.UTF8;
        }

        //写日志
        public static void Log(string level, string message)
        {
            // 获取当前时间并格式化
            string timestamp = DateTime.Now.ToString("HH:mm:ss");

            // 构造完整日志条目
            string logContent = $"[{timestamp}][{level}]: {message}";

            // 拼接完整文件路径
            string logPath = Path.Combine(Application.StartupPath, "Log.log");

            // 使用追加模式写入文件
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine(logContent);
            }

        }

        //文件写一行
        public static void FileAddLine(string content, string filePath)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(content);
            }
        }

        //连通性测试
        public static object CheckUrlConnection(string url)
        {
            // 验证URL格式有效性
            try
            {
                var uri = new Uri(url);
            }
            catch (UriFormatException)
            {
                return "unconnect";
            }

            HttpWebRequest request = null;
            Stopwatch sw = new Stopwatch();

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;     // 设置5秒超时
                request.Method = "HEAD";     // 使用HEAD方法减少数据量

                sw.Start();
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    sw.Stop();
                    return sw.ElapsedMilliseconds;
                }
            }
            catch (WebException ex)
            {
                sw.Stop();
                /* 服务器响应但返回错误状态（如404）的情况
                   仍视为连接成功，返回延迟时间 */
                if (ex.Response != null)
                {
                    return sw.ElapsedMilliseconds;
                }
                return "unconnect"; // 真正无法连接的情况
            }
            catch (Exception)
            {
                return "unconnect";
            }
            finally
            {
                request?.Abort(); // 确保释放网络资源
            }
        }

        //执行控制台命令
        public string ExecuteCommand(string command)
        {
            try
            {
                var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    CreateNoWindow = false,          // 不创建新窗口
                    UseShellExecute = false,        // 不使用系统外壳程序执行
                    RedirectStandardError = true,   // 重定向标准错误
                    RedirectStandardOutput = true   // 重定向标准输出
                };

                using (var process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();

                    // 异步读取输出流和错误流
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();  // 等待程序执行完成

                    // 组合输出结果
                    string result = string.IsNullOrEmpty(output) ? "" : output;
                    string errorResult = string.IsNullOrEmpty(error) ? "" : "\n[Error]\n" + error;

                    return $"{result}{errorResult} (ExitCode: {process.ExitCode})";
                }
            }
            catch (Exception ex)
            {
                return $"执行命令时发生异常：{ex.Message}";
            }
        }

        //检查文件
        public bool FileContainsText(string filePath, string searchText)
        {
            try
            {
                // 检查搜索文本是否有效
                if (string.IsNullOrEmpty(searchText))
                    return false;

                // 读取文件全部内容
                string fileContent = File.ReadAllText(filePath);

                // 检查内容是否包含目标文本
                return fileContent.Contains(searchText);
            }
            catch (Exception ex) when (ex is FileNotFoundException ||
                                      ex is IOException ||
                                      ex is UnauthorizedAccessException)
            {
                // 处理常见文件异常：文件不存在、无法访问或IO错误
                return false;
            }
        }

        //构造函数======================================================================================
        public Main_Window()
        {
            InitializeComponent();
            //初始化
            File.Delete(Directory.GetCurrentDirectory() + "\\Log.log");//删除原日志

            this.Text = $"{Title} {Version}";//设置窗口标题
            Log("INFO", $"成功设置Main_Window窗口标题:\"{this.Text}\"");

            if(!File.Exists(ConfigPath))//判断配置文件是否存在
            {
                Log("WARN", $"{ConfigPath}处未发现配置文件，正在初始化配置文件");
                WriteConfig(ConfigPath, "EULA", "false");//EULA同意状态
                Log("INFO", $"EULA配置项写入成功,其值为\"{ReadConfig(ConfigPath, "EULA")}\"");
                Log("INFO", $"初始化配置文件成功，配置文件路径为:\"{ConfigPath}\"");
            }

            
        }
        //事件========================================================================================

        //窗口Main_Window 加载
        private void Main_Window_Load(object sender, EventArgs e)
        {
            //EULA
            if (ReadConfig(ConfigPath, "EULA") == "false")
            {
                Log("INFO", $"检测到首次启动程序，弹出EULA提示");
                MessageBox.Show("请在接下来的窗口中，仔细阅读并同意\"最终用户协议(EULA)\"\n\n点击\"是\"则代表您同意此协议；点击\"否\"则代表您拒绝此协议并退出此程序", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (MessageBox.Show("=====[GithubConnect] 最终用户许可协议(EULA)=====\r\n欢迎使用本软件！本软件由HuaZi-华子(以下简称“开发者”）独立开发，未隶属于任何组织和公司。请仔细阅读本最终用户许可协议（以下简称“本协议”），在使用本软件之前，您应充分理解并同意本协议的所有条款。若您使用本软件，即表示您已接受本协议的约束。\r\n\r\n 一、授权许可\r\n1. 开发者授予用户一项非排他性、不可转让、有限的许可，允许用户在符合本协议条款的前提下，在个人设备上安装、使用本软件。\r\n2. 用户仅可将软件用于个人非商业用途。不可将软件用于商业目的。\r\n\r\n 三、软件使用限制\r\n1. 用户不得对软件进行反向工程、反编译、拆卸或以其他方式试图获取软件的源代码，除非适用法律明确允许且无法通过其他合法手段实现特定目的。\r\n2. 用户不得对软件进行修改、改编、翻译，不得创作软件的衍生作品，不得删除、隐匿软件中的任何版权声明或其他权利声明。\r\n3. 用户不得将软件出租、租赁、出借、分发或以其他方式转让给任何第三方，不得将软件用于分时共享服务或其他类似的商业服务模式。\r\n4. 用户不得利用软件从事任何违法、侵权或损害开发者合法权益的活动，包括但不限于传播恶意软件、侵犯知识产权、进行网络攻击等。\r\n\r\n 四、知识产权\r\n1. 软件及其所有组成部分的知识产权，包括但不限于版权、专利权、商标权等，均归开发者所有。本协议仅授予用户软件的使用许可，未转让任何知识产权。\r\n2. 用户承认软件中包含的开发者的知识产权，并同意在使用软件过程中遵守相关法律法规，尊重开发者的知识产权。\r\n\r\n 五、责任限制与免责声明\r\n1. 软件按“现状”提供，开发者不提供任何形式的明示或默示的保证，包括但不限于适销性、特定用途适用性、非侵权性的保证。软件可能存在缺陷、错误或故障，使用软件的风险由用户自行承担。\r\n2. 在任何情况下，开发者对因使用软件或无法使用软件而导致的任何直接、间接、偶然、特殊、示范或后果性的损害（包括但不限于数据丢失、业务中断、利润损失等）不承担责任，即使开发者已被告知可能发生此类损害。\r\n3. 若因软件使用导致用户对第三方承担任何责任，用户应自行承担全部责任，并使开发者免受任何损失和索赔。\r\n\r\n 六、协议的变更与终止\r\n1. 开发者有权随时修改本协议的条款。修改后的协议将在软件中或以其他适当方式公布。若用户在协议修改后继续使用软件，即表示用户接受修改后的协议。\r\n2. 若用户违反本协议的任何条款，开发者有权随时终止本协议，收回软件的使用许可。协议终止后，用户应立即停止使用软件，并删除软件的所有副本。\r\n\r\n 七、法律适用与争议解决\r\n1. 本协议的签订、履行、解释及争议解决均适用[具体法律适用地区]法律。\r\n2. 若双方在本协议的履行过程中发生争议，应首先通过友好协商解决；协商不成的，任何一方均有权向有管辖权的人民法院提起诉讼。\r\n\r\n 八、其他条款\r\n1. 本协议构成双方就软件使用的全部协议，取代双方之前关于软件使用的所有口头或书面协议。\r\n2. 若本协议的任何条款被认定为无效或不可执行，不影响其他条款的有效性和可执行性。无效或不可执行的条款应被解释为尽可能接近双方原本意图的有效条款。\r\n3. 开发者未行使或延迟行使本协议项下的任何权利，不应视为对该权利的放弃；开发者对用户违反本协议的任何豁免，不应被视为对后续违约行为的豁免。\r\n", "最终用户协议(EULA)", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    WriteConfig(ConfigPath, "EULA", "true");
                    Log("INFO", $"用户同意EULA协议，配置项EULA值为\"{ReadConfig(ConfigPath, "EULA")}\"");
                }
                else
                {
                    WriteConfig(ConfigPath, "EULA", "false");
                    Log("INFO", $"用户拒绝EULA协议，配置项EULA值为\"{ReadConfig(ConfigPath, "EULA")}\"");
                    Log("INFO", $"程序正常退出");
                    this.Close();
                }
            }


            //连通性测试
            string ConnectTest = "" + CheckUrlConnection("http://github.com");
            Log("INFO", $"连通性测试:{ConnectTest}");
            if (ConnectTest != "unconnect")
            {
                label_CnDisplay.Text = $"连通性测试:{ConnectTest}ms";
                if (int.Parse(ConnectTest) <= 100)
                {
                    label_CnDisplay.ForeColor = Color.Green;
                    Log("INFO", "连通性测试延迟于0~100区间,标签label_CnDisplay颜色设为\"绿色\"");
                }
                else if (int.Parse(ConnectTest) <= 500 && int.Parse(ConnectTest) > 100)
                {
                    label_CnDisplay.ForeColor = Color.Orange;
                    Log("INFO", "连通性测试延迟于101~500区间,标签label_CnDisplay颜色设为\"橙色\"");
                }
                else if (int.Parse(ConnectTest) <= 1000 && int.Parse(ConnectTest) > 500)
                {
                    label_CnDisplay.ForeColor = Color.OrangeRed;
                    Log("INFO", "连通性测试延迟于501~1000区间,标签label_CnDisplay颜色设为\"橙红色\"");
                }
                else
                {
                    label_CnDisplay.ForeColor = Color.Red;
                    Log("INFO", "连通性测试延迟于1001+区间,标签label_CnDisplay颜色设为\"红色\"");
                }


            }
            else
            {
                Log("INFO", $"连通性测试:超时");
                label_CnDisplay.Text = $"连通性测试:超时";
                label_CnDisplay.ForeColor = Color.Red;
                Log("INFO", "连通性测试延迟为超时,标签label_CnDisplay颜色设为\"红色\"");
            }
            //标签居中
            label_CnDisplay.Left = this.Width / 2 - label_CnDisplay.Width / 2;
            Log("INFO", $"label_CnDisplay标签居中，左方坐标为{label_CnDisplay.Left + ""}");

        }

        //窗口Main_Window 被关闭
        private void Main_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log("INFO", "程序正常退出");
        }

        //菜单栏->帮助->最终用户协议(EULA) 被点击
        private void 最终用户协议EULAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("INFO", $"用户点击菜单栏->帮助->最终用户协议(EULA)");
            MessageBox.Show("=====[GithubConnect] 最终用户许可协议(EULA)=====\r\n欢迎使用本软件！本软件由HuaZi-华子(以下简称“开发者”）独立开发，未隶属于任何组织和公司。请仔细阅读本最终用户许可协议（以下简称“本协议”），在使用本软件之前，您应充分理解并同意本协议的所有条款。若您使用本软件，即表示您已接受本协议的约束。\r\n\r\n 一、授权许可\r\n1. 开发者授予用户一项非排他性、不可转让、有限的许可，允许用户在符合本协议条款的前提下，在个人设备上安装、使用本软件。\r\n2. 用户仅可将软件用于个人非商业用途。不可将软件用于商业目的。\r\n\r\n 三、软件使用限制\r\n1. 用户不得对软件进行反向工程、反编译、拆卸或以其他方式试图获取软件的源代码，除非适用法律明确允许且无法通过其他合法手段实现特定目的。\r\n2. 用户不得对软件进行修改、改编、翻译，不得创作软件的衍生作品，不得删除、隐匿软件中的任何版权声明或其他权利声明。\r\n3. 用户不得将软件出租、租赁、出借、分发或以其他方式转让给任何第三方，不得将软件用于分时共享服务或其他类似的商业服务模式。\r\n4. 用户不得利用软件从事任何违法、侵权或损害开发者合法权益的活动，包括但不限于传播恶意软件、侵犯知识产权、进行网络攻击等。\r\n\r\n 四、知识产权\r\n1. 软件及其所有组成部分的知识产权，包括但不限于版权、专利权、商标权等，均归开发者所有。本协议仅授予用户软件的使用许可，未转让任何知识产权。\r\n2. 用户承认软件中包含的开发者的知识产权，并同意在使用软件过程中遵守相关法律法规，尊重开发者的知识产权。\r\n\r\n 五、责任限制与免责声明\r\n1. 软件按“现状”提供，开发者不提供任何形式的明示或默示的保证，包括但不限于适销性、特定用途适用性、非侵权性的保证。软件可能存在缺陷、错误或故障，使用软件的风险由用户自行承担。\r\n2. 在任何情况下，开发者对因使用软件或无法使用软件而导致的任何直接、间接、偶然、特殊、示范或后果性的损害（包括但不限于数据丢失、业务中断、利润损失等）不承担责任，即使开发者已被告知可能发生此类损害。\r\n3. 若因软件使用导致用户对第三方承担任何责任，用户应自行承担全部责任，并使开发者免受任何损失和索赔。\r\n\r\n 六、协议的变更与终止\r\n1. 开发者有权随时修改本协议的条款。修改后的协议将在软件中或以其他适当方式公布。若用户在协议修改后继续使用软件，即表示用户接受修改后的协议。\r\n2. 若用户违反本协议的任何条款，开发者有权随时终止本协议，收回软件的使用许可。协议终止后，用户应立即停止使用软件，并删除软件的所有副本。\r\n\r\n 七、法律适用与争议解决\r\n1. 本协议的签订、履行、解释及争议解决均适用[具体法律适用地区]法律。\r\n2. 若双方在本协议的履行过程中发生争议，应首先通过友好协商解决；协商不成的，任何一方均有权向有管辖权的人民法院提起诉讼。\r\n\r\n 八、其他条款\r\n1. 本协议构成双方就软件使用的全部协议，取代双方之前关于软件使用的所有口头或书面协议。\r\n2. 若本协议的任何条款被认定为无效或不可执行，不影响其他条款的有效性和可执行性。无效或不可执行的条款应被解释为尽可能接近双方原本意图的有效条款。\r\n3. 开发者未行使或延迟行使本协议项下的任何权利，不应视为对该权利的放弃；开发者对用户违反本协议的任何豁免，不应被视为对后续违约行为的豁免。\r\n", "最终用户协议(EULA)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Log("INFO", $"信息框正常退出");

        }

        //菜单栏->帮助->Github仓库* 被点击
        private void github仓库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("INFO", $"用户点击菜单栏->帮助->Github仓库*");
            Process.Start("https://github.com/bilibilihuazi/GithubConnect");
            Log("INFO", $"正常跳转Github仓库(https://github.com/bilibilihuazi/GithubConnect)");
        }

        //菜单栏->帮助->关于程序 被点击
        private void 关于程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("INFO", $"用户点击菜单栏->帮助->关于程序");
            MessageBox.Show("GithubConnect(Github连接器)\n\nHuaZi-华子 版权所有 © 2024~2025 盗版必究 此软件完全免费\n\n此软件使用.NET Framework4.8框架开发", "关于程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Log("INFO", $"信息框正常退出");
        }

        //按钮button_Fix 被点击
        private void button_Fix_Click(object sender, EventArgs e)
        {
            Log("INFO", $"用户点击按钮button_Fix");
            Log("INFO", $"开始修复连接==================================================");

            //禁用按钮
            button_Fix.Enabled = false;
            Log("INFO", $"按钮button_Fix禁用");
            button_CheckCn.Enabled = false;
            Log("INFO", $"按钮button_CheckCn禁用");

            //写入hosts文件
            if(FileContainsText(HostsPath, "20.205.243.166 github.com"))
            {
                Log("WARN", $"hosts文件已有项，无需写入");

            }
            else
            {
                FileAddLine("20.205.243.166 github.com", HostsPath);
                Log("INFO", $"hosts文件写入成功");

            }
            Log("INFO", $"hosts文件内容为:\n{File.ReadAllText(HostsPath)}");

            //刷新DNS缓存
            string flushdnsReturn = ExecuteCommand("ipconfig/flushdns");
            Log("INFO", $"成功刷新DNS缓存，其返回值为:\"\n{flushdnsReturn}\"");

            Log("INFO", "连接修复成功！");

            //启用按钮
            button_Fix.Enabled = true;
            Log("INFO", $"按钮button_Fix启用");
            button_CheckCn.Enabled = true;
            Log("INFO", $"按钮button_CheckCn启用");

            Log("INFO", $"修复连接结束==================================================");

        }

        //按钮button_CheckCn 被点击
        private void button_CheckCn_Click(object sender, EventArgs e)
        {
            Log("INFO", $"用户点击按钮button_CheckCn");
            Log("INFO", $"开始连通性测试==================================================");


            //禁用按钮
            button_Fix.Enabled = false;
            Log("INFO", $"按钮button_Fix禁用");
            button_CheckCn.Enabled = false;
            Log("INFO", $"按钮button_CheckCn禁用");

            //连通性测试
            string ConnectTest = "" + CheckUrlConnection("http://github.com");
            Log("INFO", $"连通性测试:{ConnectTest}");
            if (ConnectTest != "unconnect")
            {
                label_CnDisplay.Text = $"连通性测试:{ConnectTest}ms";
                if (int.Parse(ConnectTest) <= 100)
                {
                    label_CnDisplay.ForeColor = Color.Green;
                    Log("INFO", "连通性测试延迟于0~100区间,标签label_CnDisplay颜色设为\"绿色\"");
                }
                else if (int.Parse(ConnectTest) <= 500 && int.Parse(ConnectTest) > 100)
                {
                    label_CnDisplay.ForeColor = Color.Orange;
                    Log("INFO", "连通性测试延迟于101~500区间,标签label_CnDisplay颜色设为\"橙色\"");
                }
                else if (int.Parse(ConnectTest) <= 1000 && int.Parse(ConnectTest) > 500)
                {
                    label_CnDisplay.ForeColor = Color.OrangeRed;
                    Log("INFO", "连通性测试延迟于501~1000区间,标签label_CnDisplay颜色设为\"橙红色\"");
                }
                else
                {
                    label_CnDisplay.ForeColor = Color.Red;
                    Log("INFO", "连通性测试延迟于1001+区间,标签label_CnDisplay颜色设为\"红色\"");
                }


            }
            else
            {
                Log("INFO", $"连通性测试:超时");
                label_CnDisplay.Text = $"连通性测试:超时";
                label_CnDisplay.ForeColor = Color.Red;
                Log("INFO", "连通性测试延迟为超时,标签label_CnDisplay颜色设为\"红色\"");
            }

            //标签居中
            label_CnDisplay.Left = this.Width / 2 - label_CnDisplay.Width / 2;
            Log("INFO", $"label_CnDisplay标签居中，左方坐标为{label_CnDisplay.Left + ""}");

            //启用按钮
            button_Fix.Enabled = true;
            Log("INFO", $"按钮button_Fix启用");
            button_CheckCn.Enabled = true;
            Log("INFO", $"按钮button_CheckCn启用");

            Log("INFO", $"连通性测试结束==================================================");

        }
    }
}
