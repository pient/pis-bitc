
<%@ Page Title="资产登记" Language="C#" AutoEventWireup="true" CodeBehind="OA_PropertyEdit.aspx.cs" Inherits="PIC.Biz.Web.OA_PropertyEdit" %>

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
                title: "资产登记",
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
                    </td>
                </tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Code" name="Code"  class='validate[required]' />
                    </td>
                    <td class="pic-ui-td-caption">
                        名称
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Name" name="Name"  class='validate[required]' />
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        类型
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Type" name="Type" picctrl='select' enum="PICState['TypeEnum']" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        状态
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Status" name="Status" picctrl='select' enum="PICState['StatusEnum']" style="width: 153px"></select>
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        规格
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Spec" name="Spec"  />
                    </td>
                    <td class="pic-ui-td-caption">
                        总价
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Price" name="Price" picctrl='decimal' />
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        供应商
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Supplier" name="Supplier"  />
                    </td>
                    <td class="pic-ui-td-caption">
                        联系人
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Contact" name="Contact"  />
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        联系电话
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="ContactTel" name="ContactTel"  />
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
                        备注
                    </td>
                    <td class="pic-ui-td-data" colspan=3>
                        <textarea id="Description" name="Description" rows="4" style="width:98%"></textarea>
                    </td>
                </tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        登记人
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CreatorName" name="CreatorName" disabled=disabled  />
                    </td>
                    <td class="pic-ui-td-caption">
                        登记日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CreatedDate" name="CreatedDate" disabled=disabled  picctrl='date' />
                    </td>
				</tr>

            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：
    </div>
</asp:Content>


