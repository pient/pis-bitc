
<%@ Page Title="职称" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeTitleEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeTitleEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        var eid;

        function onPgLoad() {
            eid = $.getQueryString({ "ID": "eid" });

            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#EmployeeId").val(eid); 

                $("#CreatorName").val(PICState.UserInfo.Name);
                $("#CreatedDate").val(jQuery.dateOnly(PICState.SystemInfo.Date));
            }

            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                title: "职称",
                tbar: {
                    xtype: 'picfrmtoolbar',
                    items: [{
                        xtype: 'picsavebutton',
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
                        <input id="EmployeeId" name="EmployeeId" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        职称
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Title" name="Title" picctrl='select' enum="PICState['TitleEnum']" class="validate[required]" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        证书编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CertiNumber" name="CertiNumber"  />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        任职日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="InauguralDate" name="InauguralDate"  picctrl='date' />
                    </td>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        外语成绩
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="LanguageScore" name="LanguageScore"  />
                    </td>
                    <td class="pic-ui-td-caption">
                        取得时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="HoldDate" name="HoldDate"  picctrl='date' />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">职称报名情况</td>
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="ApplyStatus" name="ApplyStatus" rows=4 style="width: 98%;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">附件</td>
                    <td class="pic-ui-td-data" colspan=3>
                        <input id="Attachments" name="Attachments" picctrl='file' mode="multi" style="width: 98%; height:50px" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：职称
    </div>
</asp:Content>


