<%@ Page Language="C#" AutoEventWireup="true" Inherits="PIC.Portal.Web.ErrPage" %>

<html>
<head>
<title>Error</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">

<style type="text/css">

	body {
		margin:0px;
		/*FILTER:progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#FAFBFF,endColorStr=#C7D7FF);*/
		font-size:12px;
		color:#003399;
		font-family:Verdana, Arial, Helvetica, sans-serif;
	}
	
	#main{
		position: absolute; 
		top:40%;
		left:50%;
		margin-left:-350px; 
		margin-top:-150px;
	}
	
</style>

</head>
<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<form method="post" runat="server">
    <div id="main" align="center">
        <div align="center">
            <table id="__01" width="728" height="215" border="0" cellpadding="0" cellspacing="0" style="background-color:white;">
                <tr>
                    <td><img src="/portal/images/portal/error/Error_01.png" width="7" height="215" alt=""></td>
                    <td style="width:714px; height:215px; padding:10px; border-bottom:1px solid #03488b; border-top:1px solid #03488b;">
                        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                          <tr>
                            <td valign="top" width="25%" rowspan="3">
                                <img src="/portal/images/portal/error/Logo-02.png" width="150" />
                            </td>
                            <td style="width:10px; border-left:1p solid #70aff6;" rowspan="3">&nbsp</td>
                            <td style="color:Red; font-weight:bold; font-size:14px;">&nbsp;
                                <%if (sc == "404")
                                  { %>
                                  错误：页面未找到。
                                <%} %>
                            </td>
                          </tr>
                          <tr>
                            <td style="font-size:12px; padding:5px;" valign="top">&nbsp;
                                <%if (sc == "404")
                                  { %>
                                  <p>对不起，页面在系统中不存在。</p>
                                  <p>页面已经被删除或被错误录入，若问题持续，请联系管理员。</p>
                                <%} %>
                            </td>
                          </tr>
                          <tr>
                            <td>&nbsp;
                            </td>
                          </tr>
                        </table>
                    </td>
                    <td><img src="/portal/images/portal/error/Error_03.png" width="7" height="215" alt=""></td>
                </tr>
            </table>
        </div>
    </div>
</form>
</body>
</html>
