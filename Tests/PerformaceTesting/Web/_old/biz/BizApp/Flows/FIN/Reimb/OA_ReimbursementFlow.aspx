
<%@ Page Title="行政（月结）费用报销" Language="C#" AutoEventWireup="true" Async="true" CodeBehind="OA_ReimbursementFlow.aspx.cs" Inherits="PIC.Biz.Web.OA_ReimbursementFlow" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
<script type="text/javascript" src="http://localhost:4612/js/pgctrl-ext-form.js"></script>
    <script type="text/javascript">

        var HistoryWinStyle = CenterWin("width=950,height=500,scrollbars=yes");
        var HistoryPageUrl = "OA_ReimbHistoryList.aspx";

        var PicWinStyle = CenterWin("width=880,height=700,scrollbars=yes");
        var PicPageUrl = "FlowPicView.aspx";

        var TypeEnum = { Admin: '综合', Functional: '分摊' };

        var store, grid;
        var act, isreceive, isassign;

        function onPgLoad() {
            act = $.getQueryString({ ID: 'act' });

            isreceive = (act == 'receive' && pgOperation == 'r' && $("#Status").val() == 'Assigned');
            isassign = ((act == 'assign' && $("#Status").val() == 'New') || pgOperation == "c" || pgOperation == "u")

            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                PICCtrl["FeeDate"].setValue(jQuery.dateOnly(PICState.SystemInfo.Date));
                PICCtrl["CreatorName"].setValue(PICState.UserInfo.Name);
                PICCtrl["CreatedDate"].setValue(jQuery.dateOnly(PICState.SystemInfo.Date));
            }

            InitItemGrid();
            
            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                items: { xtype: 'picfrmpanel',
                    title: "行政（月结）费用报销",
                    applyTo: PICFrm.dom,
                    border: false,
                    layout: 'column',
                    tbar: {
                        xtype: 'picfrmtoolbar',
                        items: [{
                            xtype: 'picsavebutton',
                            id: 'btnSave'
                        }, {
                            iconCls: 'pic-icon-submit',
                            picexecutable: true,
                            text: '提交',
                            id: 'btnSubmit'
                        }, {
                            iconCls: 'pic-icon-delete',
                            picexecutable: true,
                            text: '删除',
                            hidden: (pgOp == "c"),
                            id: 'btnDelete'
                        }, '-', 'cancel', '->', '-', {
                            iconCls: 'pic-icon-time',
                            text: '历史查看',
                            id: 'btnHistory',
                            handler: function () {
                                OpenWin(HistoryPageUrl, '_blank', HistoryWinStyle);
                            }
                        }, {
                            iconCls: 'pic-icon-arrow-round',
                            text: '流程图',
                            id: 'btnFlow',
                            handler: function () {
                                OpenWin(PicPageUrl, '_blank', PicWinStyle);
                            }
                        }, '-', 'help']
                    },
                    items: [{ xtype: 'picpanel', border: false, contentEl: 'editDiv1' },
                    { xtype: 'picpanel', border: false, items: grid },
                    { xtype: 'picpanel', border: false, contentEl: 'editDiv2' },
                    { xtype: 'picpanel', border: false, contentEl: 'descDiv'}]
                }
            });

            //绑定按钮验证
            FormValidationBind('btnSave', DoSave);
            FormValidationBind('btnDelete', DoDelete);
            FormValidationBind('btnSubmit', DoSubmit);
            FormValidationBind('btnAgree', function () { DoAction('Pass'); });
            FormValidationBind('btnReject', function () { DoAction('Reject'); });

            // RenderItemGrid();

            /*PICCtrl["Type"].facade.on("change", function () {
                
            });*/
        }

        function DoSave() {
            commitItemsChange();

            PICFrm.submit(pgAction, {}, null, SubFinish);
        }

        function DoDelete() {
            commitItemsChange();

            PICFrm.submit('delete', {}, null, SubFinish);
        }

        //验证成功执行保存方法
        function DoSubmit() {
            var recs = store.getRange();
            var type = PICCtrl["Type"].getValue();

            if ("Functional".equals(type)) {
                if (recs.length <= 0) {
                    alert("分摊报销，必须提供包含部门的条目信息。");
                    return;
                }

                // 若为分摊审批，则所有条目部门项不能为空
                for (var i = 0; i < recs.length; i++) {
                    var trec = recs[i];
                    if (!trec.get('DepartmentId')) {
                        // PICDlg.show("分摊审批，所有条目必须提供部门信息。");
                        alert("分摊报销，所有条目必须提供部门信息。");
                        var idx = store.indexOf(trec);

                        // grid.getSelectionModel().selectRow(idx);
                        grid.startEditing(idx, 6);
                        return false;
                    }
                }
            } else {
                for (var i = 0; i < recs.length; i++) {
                    var trec = recs[i];
                    if (trec.get('DepartmentId')) {
                        if (!confirm("条目中有部门信息，确定为综合报销？")) {
                            PICCtrl["Type"].facade.focus();

                            return false;
                        } else {
                            grid.startEditing(idx, 6);
                        }
                    }
                }
            }

            commitItemsChange();
            
            PICFrm.submit('submit', {}, null, SubFinish);
        }

        function DoAction(actionCode, actionId, comments, callback) {
            PICFrm.submit('action', { ActionCode: actionCode, ActionId: actionId, Comments: comments }, null, callback);
        }

        function SubFinish(args) {
            RefreshClose();
        }

        // 控制点编辑表
        function InitItemGrid() {
            var cpstr = $("#Items").val();
            var cprecords = ($.getJsonObj(cpstr) || {}).Items || [];

            var cpdata = { total: cprecords.length, records: cprecords };

            store = new Ext.ux.PICGridStore({
                idProperty: 'Index',
                data: cpdata,
                fields: [
			    { name: 'Index' },
			    { name: 'Title' },
			    { name: 'Fee' },
			    { name: 'Date' },
			    { name: 'DepartmentId' },
			    { name: 'DepartmentName' },
			    { name: 'IsReceipt' },
			    { name: 'Comments' }
			]
            });

            grid = new Ext.ux.grid.PICEditorGridPanel({
                id: 'grid',
                applyTo: 'div_cp',
                store: store,
                title: '费用分摊明细',
                // width: 670,
                height: 300,
                pgbar: false,
                tlitems: [/*{
			            text: '确定',
			            iconCls: 'pic-icon-submit',
			            picexecutable: true,
			            handler: function () {
			                commitPCChange();
			            }
			        }, */{
			        bttype: 'add',
			        handler: function () {
			            var SubRecord = store.recordType;
			            var p = new SubRecord({ Fee: 0, Date: new Date() });
			            grid.stopEditing();

			            var insRowIdx = store.data.length;
			            store.insert(insRowIdx, p);
			            grid.startEditing(insRowIdx, 1);
			        }
			    }, {
			        bttype: 'delete',
			        handler: function () {
			            var recs = grid.getSelectionModel().getSelections();
			            var d_recs = [];

			            if (!recs || recs.length <= 0) {
			                PICDlg.show("请先选择要删除的记录！");
			                return;
			            }

			            if (confirm("确定删除所选记录？")) {
			                $.each(recs, function () {
			                    store.remove(this);
			                });
			            }
			        }
			    }, '-', 'excel'],
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
                    { id: 'Title', dataIndex: 'Title', header: "标题", editor: { xtype: 'textfield', minLength: 0, maxLength: 100 }, width: 120 },
                    { id: 'Fee', dataIndex: 'Fee', header: "费用", editor: { xtype: 'numberfield' }, width: 80 },
                    { id: 'Date', dataIndex: 'Date', header: "日期", editor: { xtype: 'datefield' }, width: 100, renderer: Ext.util.Format.dateRenderer('Y-m-d') },
                    { id: 'DepartmentName', dataIndex: 'DepartmentName', header: "部门", editor: { xtype: 'picgridgroupselector', idindex: 'DepartmentId', nameindex: 'DepartmentName', params: { mode: 'org'} }, width: 80 },
                    { id: 'Comments', dataIndex: 'Comments', header: "注释", editor: { xtype: 'textarea', height: 100 }, width: 180 }
                    ],
                autoExpandColumn: 'Comments',
                autoExpandMin: 150
            });

            var status = PICFrmData["Status"];
            if ('r'.equals(pgOperation)) {  // 设为只读
                grid.on('beforeedit', function () { return false; });
            }

            grid.on('afteredit', function (ds, e, b) {
                switch (ds.field) {
                    case 'Fee':
                        var totfee = 0;
                        var rec = ds.record;
                        var fee = parseFloat(rec.get("Fee") == null ? 0 : rec.get("Fee"));

                        var recs = store.getRange();
                        $.each(recs, function () {
                            totfee += (this.get("Fee") || 0);
                        });

                        PICCtrl['Fee'].setValue(totfee);
                        break;
                    case 'DepartmentName':
                        var editor = ds.grid.lastActiveEditor;
                        if (editor && editor.field && editor.field.extdata) {
                            var extdata = editor.field.extdata;

                            var rec = ds.record;
                            rec.set('DepartmentId', extdata.GroupID);
                        }
                        break;
                }
            });
        }

        function commitItemsChange() {
            var recs = store.getRange();
            var cprecs = [];
            var datastr = null;
            $.each(recs, function () {
                cprecs.push(this.data);
            });

            var cpdata = { Items: cprecs }
            cpdatastr = $.getJsonString(cpdata);
            $("#Items").val(cpdatastr);
            store.commitChanges();
        }

        function OnTypeChange(ctrl) {
            // alert();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">    
    <div id="editDiv1" align="center">
        <table class="pic-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                        <input id="Status" name="Status" />
                        <input id="Items" name="Items" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        名称
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Name" name="Name" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        编号
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Code" name="Code" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="FeeDate" name="FeeDate"  class="validate[required]" picctrl='date' config="{ format: 'Y-m-d' }" />
                    </td>
                    <td class="pic-ui-td-caption">
                        费用
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Fee" name="Fee"  class="validate[required]" picctrl='decimal' />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        类型
                    </td>
                    <td class="pic-ui-td-data">
                        <select id="Type" name="Type" picctrl='select' enum="TypeEnum" class="validate[required]" onchange='OnTypeChange(this)' style="width: 153px"></select>
                    </td>
                    <!--<td class="pic-ui-td-caption">
                        行政经理
                    </td>
                    <td class="pic-ui-td-data">
                        <input picctrl='popup' id="Text1" name="PMName" class="validate[required]"
                            relateid="PMId" popurl="/portal/CommonPages/Select/UsrSelect/MUsrSelect.aspx?seltype=single" 
                            popparam="PMId:UserID;PMName:Name" popstyle="width=450,height=450"
                            class="text ui-widget-content" />
                        <input type="hidden" id="Hidden1" name="PMId" />
                    </td>-->
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        注释
                    </td>
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="Comments" name="Comments" rows=4 style="width: 98%;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption" >
                        附件
                    </td>
                    <td class="pic-ui-td-data" colspan="3">
                        <input id="Attachments" name="Attachments" picctrl='file' mode="multi" style="width: 98%;" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="div_cp" name="div_cp" style="text-align:left"></div>
    <div id="editDiv2" align="center">
        <table class="pic-ui-table-edit">
            <tbody>
                <tr>
                    <td class="pic-ui-td-caption">
                        编制人
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CreatorName" name="CreatorName" disabled  />
                    </td>
                    <td class="pic-ui-td-caption">
                        编制时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="CreatedDate" name="CreatedDate" disabled />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：行政（月结）费用报销
    </div>
</asp:Content>


