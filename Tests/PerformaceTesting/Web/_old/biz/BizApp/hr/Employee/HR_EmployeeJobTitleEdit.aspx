
<%@ Page Title="从业资质" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeJobTitleEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeJobTitleEdit" %>

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
                title: "从业资质",
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

        function LoadPic(photo, img) {
            var photoobj = $('#' + photo);
            var imgobj = $('#' + img);

            if (photoobj.val()) {
                var url = PICGetDownloadUrlByFullName(photoobj.val());

                imgobj.attr('src', url);
            } else {
                switch (photo) {
                    case "Picture":
                        imgobj.attr('src', BLANK_PIC_URL);
                        break;
                }
            }
        }

        function DisplayPic(photo) {
            var photoobj = $('#' + photo);
            if (photoobj.val()) {
                PICOpenDownloadWin(photoobj.val());
            }
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
                        名称
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Name" name="Name" picctrl='select' enum="PICState['TitleEnum']" class="validate[required]" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption" rowspan=4>
                        证书
                    </td>
                    <td class="pic-ui-td-data" rowspan=4>
                        <img id="imgPicture" onclick="DisplayPic('Picture');" style="cursor:hand;" height="100" width="100" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        证书号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Number" name="Number"  />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        取资日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="HoldDate" name="HoldDate"  picctrl='date' />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        取资方式
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="HoldWay" name="HoldWay"  />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        有效期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="ValidityDate" name="ValidityDate"  picctrl='date' />
                    </td>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Picture" name="Picture" onpropertychange="LoadPic('Picture', 'imgPicture');" picctrl="file" mode='single' />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        评审日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="AuditDate" name="AuditDate"  picctrl='date' />
                    </td>
                    <td class="pic-ui-td-caption">
                        注册日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="RegisterDate" name="RegisterDate"  picctrl='date' />
                    </td>
                <tr>
                    <td class="pic-ui-td-caption">
                        发证机构
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="RegisterPlace" name="RegisterPlace"  />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">附件</td>
                    <td class="pic-ui-td-data" colspan=3>
                        <input id="Attachments" name="Attachments" picctrl='file' mode="multi" style="width: 98%; height:50px" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">备注</td>
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="Memo" name="Memo" rows=4 style="width: 98%;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">执业资质报考情况</td>
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="Textarea1" name="ApplyStatus" rows=4 style="width: 98%;"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：从业资质
    </div>
</asp:Content>


