<%@ Page Title="编辑" Language="C#" AutoEventWireup="true" CodeBehind="GroupEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.GroupEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var StatusEnum = { 1: "启用", 0: "停用" };

    var viewport;
    var id, type, title;

    function onPgLoad() {

        Ext.create('PIC.view.setup.org.GroupEdit');

//        id = $.getQueryString({ ID: 'id' });
//        type = $.getQueryString({ ID: 'type' });
//        title = $.getQueryString({ ID: 'title', DefaultValue: '编辑' });

//        document.title = title;

//        setPgUI();
    }

    function setPgUI() {
        if (type || type === 0) {
            $('#Type').val(type);
            $('#Type').attr("disabled", true);
        }

        // 页面视图
        viewport = new Ext.ux.PICFrmViewport({
            title: title,
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
        PICReturnPopupValue({ id: id, op: pgOperation });
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="editDiv" align="center">
        <table class="pic-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="GroupID" name="GroupID" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        名称
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Name" name="Name" class="validate[required, length[0,50]]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Code" name="Code" class="validate[required, length[0,50]]" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        类型
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Type" name="Type" picctrl='select' enum='PICState["GrpTypeEnum"]' class="validate[required]" style="width: 100px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        状态
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Status" name="Status" picctrl='select' enum="StatusEnum" class="validate[required]" style="width: 100px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        排序号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="SortIndex" name="SortIndex" class="validate[custom[onlyInteger]]" />
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
                        描述
                    </td>
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="Description" rows="5"  style=" width:98%"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：系统用户组编辑
    </div>
</asp:Content>
