# GithubConnect
## Operation Principle

This software **only modifies the hosts file and flushes the DNS cache**, without using any *VPN tools (proxies, bypass tools, etc.)*.<br>  
When accessing the domain `https://github.com`, there is a process of **resolving the domain to an IP address**. Connection failures to GitHub are typically caused by issues in this step.<br><br>  

When the user clicks the **Repair Connection** button, the program adds a new entry `20.205.243.166 github.com` to the **hosts** file, ensuring the domain `https://github.com` resolves to `20.205.243.166`. It then executes the console command `ipconfig /flushdns` to refresh the DNS resolution cache.<br><br><br>  

***Note: This method does not guarantee 100% success. It is only effective for intermittent connection issues. For completely unreachable cases, please use other VPN tools.***  

## Runtime Dependencies  
This software is developed using the **.NET Framework 4.8**. If the .NET Framework is not installed, please follow the in-app instructions to install it.<br>  

## Associated Files  
The program is a **single-file executable**. After running, it generates `config.ini` and `Log.log` files. The `config.ini` file stores configuration settings, while `Log.log` contains runtime logs for debugging purposes.<br>  

## References  
[Operation Principle](https://blog.csdn.net/weixin_43804496/article/details/131475204)<br>  