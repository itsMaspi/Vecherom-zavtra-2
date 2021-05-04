using System.Collections;
using System.Collections.Generic;

public class UserCfgRequest
{
	public int UserID { get; set; }
	public string Config { get; set; }

	public UserCfgRequest(int UserID, string Config)
	{
		this.UserID = UserID;
		this.Config = Config;
	}
}
