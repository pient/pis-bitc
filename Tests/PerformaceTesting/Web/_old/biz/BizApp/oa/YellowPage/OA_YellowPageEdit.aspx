
<%@ Page Title="企业黄页" Language="C#" AutoEventWireup="true" CodeBehind="OA_YellowPageEdit.aspx.cs" Inherits="PIC.Biz.Web.OA_YellowPageEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var id;

        function onPgLoad() {
            id = $.getQueryString({ ID: 'id' });

            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {                
                $("#CreatorName").val(PICState.UserInfo.Name);
                $("#CreatedDate").val(jQuery.dateOnly(PICState.SystemInfo.Date));
            }

            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                title: "企业黄页",
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
            PIC.PopUp.ReturnValue({ id: id, op: pgOperation });
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
                        名称
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Name" name="Name"  class='validate[required]'  />
                    </td>
                    <td class="pic-ui-td-caption">
                        排序号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="SortIndex" name="SortIndex" picctrl="Integer" />
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        职位
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Position" name="Position"  />
                    </td>
                    <td class="pic-ui-td-caption">
                        电话
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Phone" name="Phone"  />
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        手机
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Mobile" name="Mobile"  />
                    </td>
                    <td class="pic-ui-td-caption">
                        电子邮件
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Email" name="Email"  />
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        传真
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Fax" name="Fax"  />
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
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="Description" name="Description" rows="5"  style=" width:98%"></textarea>
                    </td>
				</tr>
				<tr>
                    <td class="pic-ui-td-caption">
                        创建人
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CreatedDate" name="CreatedDate" disabled  picctrl='date' />
                    </td>
                    <td class="pic-ui-td-caption">
                        创建人姓名
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CreatorName" name="CreatorName" disabled  />
                    </td>
				</tr>

            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：企业黄页信息编辑
    </div>
</asp:Content>


