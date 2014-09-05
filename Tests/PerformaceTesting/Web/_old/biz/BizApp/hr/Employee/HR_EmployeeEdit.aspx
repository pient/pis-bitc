
<%@ Page Title="员工基本信息" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var origdeptid = null;

        function onPgLoad() {
            origdeptid = $("#DepartmentId").val();

            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreatorName").val(PICState.UserInfo.Name);
                $("#CreatedDate").val(jQuery.dateOnly(PICState.SystemInfo.Date));

                $("#Region").val("SD");
                $("#People").val("HAN");
                $("#EducationHistory").val("BK");
                $("#InnerTitle").val("AssistedEngineer");
            }

            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                title: '员工基本信息',
                tbar: {
                    xtype: 'picfrmtoolbar',
                    items: [{
                        xtype: 'picsavebutton',
                        id: 'btnSubmit'
                    }, 'cancel', '-', '->', '-', 'help']
                }
            });

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            PICFrm.submit(pgAction, { origdeptid: origdeptid }, null, SubFinish);
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
                    case "Signature":
                        imgobj.attr('src', BLANK_PIC_URL);
                        break;
                    default:
                        imgobj.attr('src', "/portal/images/portal/def_usr.png");
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
    <fieldset style="margin:5px;">
    <legend style="margin:5px; font-size:14px;"><b>&nbsp;基本信息&nbsp;</b></legend>
        <table class="pic-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        员工编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Code" name="Code" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption" rowspan=5>
                        员工照片
                    </td>
                    <td class="pic-ui-td-data" rowspan=5>
                        <img id="imgPhoto" onclick="DisplayPic('Photo');" style="cursor:hand;" height="100" width="100" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        档案编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="FileCode" name="FileCode" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        姓名
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Name" name="Name" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        曾用名
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="UsedName" name="UsedName" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        员工类型
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Type" name="Type" picctrl='select' enum="PICState['TypeEnum']" class="validate[required]" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        状态
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Status" name="Status" picctrl='select' enum="PICState['StatusEnum']" class="validate[required]" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Photo" name="Photo" onpropertychange="LoadPic('Photo', 'imgPhoto');" picctrl="file" mode='single' style="width:230px;" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        工作地点
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Region" name="Region" picctrl='select' enum="PICState['RegionEnum']" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        考勤类型
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="AttendanceType" name="AttendanceType" picctrl='select' enum="PICState['AttendanceTypeEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        出生年月
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Birthday" name="Birthday" picctrl="date" />
                    </td>
                    <td class="pic-ui-td-caption">
                        性别
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Sex" name="Sex" picctrl='select' enum="PICState['SexEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        身份证号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="IDNumber" name="IDNumber" />
                    </td>
                    <td class="pic-ui-td-caption">
                        户口所在地
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="HouseholdAddress" name="HouseholdAddress" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        民族
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="People" name="People" picctrl='select' enum="PICState['PeopleEnum']" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        籍贯
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="NativePlace" name="NativePlace" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        毕业院校
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="School" name="School" />
                    </td>
                    <td class="pic-ui-td-caption">
                        所学专业
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="SchoolMajor" name="SchoolMajor" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        毕业时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="GraduatedDate" name="GraduatedDate" picctrl="date" />
                    </td>
                    <td class="pic-ui-td-caption">
                        学历
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="EducationHistory" name="EducationHistory" picctrl='select' enum="PICState['EducationEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        合同开始日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="ContractStartDate" name="ContractStartDate" picctrl="date" />
                    </td>
                    <td class="pic-ui-td-caption">
                        合同终止日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="ContractEndDate" name="ContractEndDate" picctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        报到日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="InauguralDate" name="InauguralDate" picctrl="date" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        转正日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="RegularizedDate" name="RegularizedDate" picctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        所属部门
                    </td>
                    <td class="pic-ui-td-data">
                        <input picctrl='popup' readonly id="DepartmentName" name="DepartmentName" relateid="DepartmentId"
                            popurl="/portal/CommonPages/Select/GrpSelect/MGrpSelect.aspx?seltype=single&mode=org" 
                            popparam="DepartmentId:GroupID;DepartmentName:Name" popstyle="width=450,height=450"
                            class="validate[required]" style="width: 153px" />
                        <input type="hidden" id="DepartmentId" name="DepartmentId" />
                    </td>
                    <td class="pic-ui-td-caption">
                        职称
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="InnerTitle" name="InnerTitle" picctrl='select' enum="PICState['InnerTitleEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        移动电话
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Mobile" name="Mobile" />
                    </td>
                    <td class="pic-ui-td-caption">
                        家庭电话
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="HomePhone" name="HomePhone" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        办公电话1
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Phone" name="Phone" />
                    </td>
                    <td class="pic-ui-td-caption">
                        办公电话2
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Phone2" name="Phone2" />
                    </td>
                </tr>
                <tr>
                    <!--<td class="pic-ui-td-caption">
                        上司
                    </td>
                    <td class="pic-ui-td-data">
                        <input picctrl='popup' readonly id="SupervisorName" name="SupervisorName" relateid="SupervisorId"
                            popurl="/portal/CommonPages/Select/UsrSelect/MUsrSelect.aspx?seltype=single" 
                            popparam="SupervisorId:UserID;SupervisorName:Name" popstyle="width=450,height=450"
                            class="text ui-widget-content" style="width: 60%"/>
                        <input type="hidden" id="SupervisorId" name="SupervisorId" />
                    </td>-->
                    <td class="pic-ui-td-caption">
                        邮政编码
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Post" name="Post" />
                    </td>
                    <td class="pic-ui-td-caption">
                        E-mail
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Email" name="Email" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        联系地址
                    </td>
                    <td class="pic-ui-td-data" colspan="3">
                        <input id="ContactAddress" name="ContactAddress" style="width:98%" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <fieldset style="margin:5px;">
    <legend style="margin:5px; font-size:14px;"><b>&nbsp;扩展信息&nbsp;</b></legend>
        <table class="pic-ui-table-edit">
            <tbody>
                <tr>
                    <td class="pic-ui-td-caption">
                        所属专业
                    </td>
                    <td class="pic-ui-td-data">
                        <!--<input picctrl='popup' readonly id="Text26" name="MajorName" relateid="MajorId"
                            popurl="/portal/CommonPages/Select/GrpSelect/MGrpSelect.aspx?seltype=single&mode=org" 
                            popparam="MajorId:Id;MajorName:Name" popstyle="width=450,height=450"
                            class="text ui-widget-content" style="width: 60%"/>
                        <input type="hidden" id="MajorId" name="MajorId" class="text ui-widget-content" />-->
                        <select id="MajorCode" name="MajorCode" picctrl='select' enum="PICState['MajorEnum']" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        （设、校、审）岗位资格
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="PostQualification" name="PostQualification" picctrl='select' enum="PICState['PostQualEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        职务（显示）
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="DutyForShow" name="DutyForShow" />
                    </td>
                    <td class="pic-ui-td-caption">
                        原工作单位
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="OrigDepartment" name="OrigDepartment" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        试用期待遇
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="TrailPay" name="TrailPay" picctrl="decimal" />
                    </td>
                    <td class="pic-ui-td-caption">
                        转正月薪
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="FormalPay" name="FormalPay" picctrl="decimal" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        调薪情况
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="PayAdjustment" name="PayAdjustment" />
                    </td>
                    <td class="pic-ui-td-caption">
                        是否缴纳综合保险
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="GeneralSafety" name="GeneralSafety" picctrl='select' enum="PICState['BooleanEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        社会保险
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Safety" name="Safety" />
                    </td>
                    <td class="pic-ui-td-caption">
                        社保编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="SafetyCode" name="SafetyCode" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        公积金
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Fund" name="Fund" />
                    </td>
                    <td class="pic-ui-td-caption">
                        公积金编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="FundCode" name="FundCode" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        婚姻状况
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="MarriageStatus" name="MarriageStatus" picctrl='select' enum="PICState['MarriageStatusEnum']" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        配偶姓名
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="SpouseName" name="SpouseName" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        政治面貌
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="PoliticalStatus" name="PoliticalStatus" picctrl='select' enum="PICState['PoliticalStatusEnum']" style="width: 153px"></select>
                    </td>
                    <td class="pic-ui-td-caption">
                        劳动手册已取
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="ManualLabor" name="ManualLabor" picctrl='select' enum="PICState['BooleanEnum']" style="width: 153px"></select>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        居住证有效期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="ResidenceValid" name="ResidenceValid" />
                    </td>
                    <td class="pic-ui-td-caption">
                        录用
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="HireStatus" name="HireStatus" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        备注
                    </td>
                    <td class="pic-ui-td-data" colspan=3>
                        <textarea id="Memo" name="Memo" rows="5" style="width:98%"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        身份证（正面）
                    </td>
                    <td class="pic-ui-td-data">
                        <img id="imgIDPhotoFront" onclick="DisplayPic('IDPhotoFront');" style="cursor:hand;" height="100" width="100" />
                    </td>
                    <td class="pic-ui-td-caption">
                        身份证（反面）
                    </td>
                    <td class="pic-ui-td-data">
                        <img id="imgIDPhotoBack" onclick="DisplayPic('IDPhotoBack');" style="cursor:hand;" height="100" width="100" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="IDPhotoFront" name="IDPhotoFront" onpropertychange="LoadPic('IDPhotoFront', 'imgIDPhotoFront');" picctrl="file" mode='single' style="width:230px;" />
                    </td>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="IDPhotoBack" name="IDPhotoBack" onpropertychange="LoadPic('IDPhotoBack', 'imgIDPhotoBack');" picctrl="file" mode='single' style="width:230px;" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        签名
                    </td>
                    <td class="pic-ui-td-data">
                        <img id="imgSignature" onclick="DisplayPic('Signature');" style="cursor:hand;"  height="100" width="100" />
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
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Signature" name="Signature" onpropertychange="LoadPic('Signature', 'imgSignature');" picctrl="file" mode='single' style="width:230px;" />
                    </td>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    
    </div>
    <br />
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：员工基本信息编辑
    </div>
</asp:Content>


