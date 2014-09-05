
<%@ Page Title="员工合同" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeContractEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeContractEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        
        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {                
                $("#CreatorName").val(PICState.UserInfo.Name);
                $("#CreatedDate").val(jQuery.dateOnly(PICState.SystemInfo.Date));
            }

            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                tbar: {
                    xtype: 'picfrmtoolbar',
                    items: [{
                        xtype: 'picsavebutton',
                        id: 'btnSubmit'
                    }, '-', '->', '-', 'help']
                }
            });

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            PICFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            // RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    
    <div id="editDiv" align="center">
        <table class="pic-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        编码
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Code" name="Code" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        类型
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Type" name="Type" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        开始时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="BeginDate" name="BeginDate" picctrl="date" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        结束时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="EndDate" name="EndDate" picctrl="date" class="validate[required]" />
                    </td>
                </tr>
                <tr width="100%">
                    <td class="pic-ui-td-caption" >
                        录入人
                    </td>
                    <td class="pic-ui-td-data">
                        <input disabled id="CreatorName" name="CreatorName" />
                    </td>
                    <td class="pic-ui-td-caption" >
                        录入日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input disabled id="CreatedDate" name="CreatedDate" dateonly="true" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：员工合同信息编辑
    </div>
</asp:Content>


