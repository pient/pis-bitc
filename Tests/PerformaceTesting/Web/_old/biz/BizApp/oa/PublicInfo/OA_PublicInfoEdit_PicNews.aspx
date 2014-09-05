
<%@ Page Title="公共信息-图片新闻" Language="C#" AutoEventWireup="true" CodeBehind="OA_PublicInfoEdit_PicNews.aspx.cs" Inherits="PIC.Biz.Web.OA_PublicInfoEdit_PicNews" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        var type, qrytype;

        function onFrmLoad() {
            qrytype = $.getQueryString({ "ID": "type" });

            if (pgOperation == "c" || pgOperation == "cs") {
                $("#Grade").val("Normal");

                $("#AuthorName").val(PICState.UserInfo.Name);
                $("#CreatedDate").val(jQuery.dateOnly(PICState.SystemInfo.Date));

                $("#IsExpired").val("N");
                $("#IsPopup").val("N");

                $("#Type").val('PictureNews');
                $("#Type").attr('disabled', false);
            }
        }

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                title: '公共信息-图片新闻',
                tbar: {
                    xtype: 'picfrmtoolbar',
                    items: [{
                        xtype: 'picsavebutton',
                        text: '保存',
                        id: 'btnSubmit'
                    }, {
                        iconCls: 'pic-icon-publish',
                        text: '发布',
                        picexecutable: true,
                        id: 'btnPublish'
                    }, '-', 'cancel', '->', '-', 'help']
                }
            });

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            FormValidationBind('btnPublish', SuccessPublish);
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            PICFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SuccessPublish() {
            PICFrm.submit(pgAction, { optype: 'publish' }, null, SubFinish);
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
                        编码
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Code" name="Code" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        标题
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Title" name="Title" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        类型
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Type" name="Type" picctrl='select' enum="PICState['TypeEnum']" class="validate[required]" disabled=disabled style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        级别(重要程度)
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Grade" name="Grade" picctrl='select' enum="PICState['GradeEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        设为过期
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="IsExpired" name="IsExpired" picctrl='select' enum="PICState['BooleanEnum']" class="validate[required]" style="width: 153px"></select>
                        <!--<input id="IsExpired" name="IsExpired" picctrl='checkbox' chktype="boolean" />-->
                    </td>
                    <td class="pic-ui-td-caption">
                        过期时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="ExpireDate" name="ExpireDate" picctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        门户弹出
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="IsPopup" name="IsPopup" picctrl='select' enum="PICState['BooleanEnum']" class="validate[required]" style="width: 153px"></select>
                        <!--<input id="IsPopup" name="IsPopup" picctrl='checkbox' chktype="boolean" />-->
                    </td>
                    <td class="pic-ui-td-caption">
                        弹出截至时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="PopupEndDate" name="PopupEndDate" picctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        关键字
                    </td>
                    <td class="pic-ui-td-data" colspan="3">
                        <input id="Keywords" name="Keywords" style="width:98%"/>
                    </td>
                </tr>
                <!--<tr>
                    <td class="pic-ui-td-caption" style="text-align:left;" colspan="4">
                        &nbsp;
                    </td>
                </tr>-->
                <tr id="tr_picture">
                    <td class="pic-ui-td-caption">
                        图片
                    </td>
                    <td class="pic-ui-td-data" colspan=3>
                        <input id="Picture" name="Picture" picctrl='file' mode="single" class="validate[required]" Filter="图片文件 (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png" style="width:98%;" />
                    </td>
                </tr>
                <tr width="100%">
                    <td class="pic-ui-td-caption" >
                        录入人
                    </td>
                    <td class="pic-ui-td-data">
                        <input disabled id="AuthorName" name="AuthorName" />
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
        表单描述：公共信息编辑
    </div>
</asp:Content>


