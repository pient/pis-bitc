
<%@ Page Title="人员资质" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeQualificationEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeQualificationEdit" %>

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
                title: "人员资质",
                tbar: {
                    xtype: 'picfrmtoolbar',
                    items: [{
                        xtype: 'picsubmitbutton',
                        text: '提交',
                        id: 'btnSubmit'
                    }, '-', 'cancel', '->', '-', 'help']
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
            RefreshClose();
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
                        EmployeeId
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="EmployeeId" name="EmployeeId"  />
                    </td>
                    <td class="pic-ui-td-caption">
                        Qualification
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Qualification" name="Qualification"  />
                    </td>
				</tr>

				<tr>
                    <td class="pic-ui-td-caption">
                        Memo
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Memo" name="Memo"  />
                    </td>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：人员资质
    </div>
</asp:Content>


