
<%@ Page Title="公共信息" Language="C#" AutoEventWireup="true" CodeBehind="OA_PublicInfoList.aspx.cs" Inherits="PIC.Biz.Web.OA_PublicInfoList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=700,height=650,scrollbars=yes");
    var EditPageUrl = "OA_PublicInfoEdit.aspx";

    var EditWinStyle_PicNews = CenterWin("width=700,height=350,scrollbars=yes");
    var EditPageUrl_PicNews = "OA_PublicInfoEdit_PicNews.aspx";

    var store, grid, viewport;

    var type, op, title = "公共信息管理";

    function onPgLoad() {
        type = $.getQueryString({ "ID": "type" }) || "None";
        op = $.getQueryString({ "ID": "op" });

        if ('PictureNews'.equals(type)) {
            EditWinStyle = EditWinStyle_PicNews;
            EditPageUrl = EditPageUrl_PicNews
        }

        EditPageUrl = $.combineQueryUrl(EditPageUrl, { type: type });

        if (PICState['TypeEnum'][type]) {
            title = '公共信息管理 - ' + PICState['TypeEnum'][type];
        }
        
        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'OA_PublicInfoList',
            idProperty: 'Id',
            loadargs: { type: type, op: op },
            fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Title' },
			{ name: 'Type' },
			{ name: 'Keywords' },
			{ name: 'AuthorId' },
			{ name: 'AuthorName' },
			{ name: 'Content' },
			{ name: 'Grade' },
			{ name: 'PublisherId' },
			{ name: 'PublisherName' },
			{ name: 'GroupId' },
			{ name: 'GroupName' },
			{ name: 'Status' },
			{ name: 'Clicks' },
			{ name: 'IsPopup' },
			{ name: 'PopupEndDate' },
			{ name: 'PublishDate' },
			{ name: 'ExpireDate' },
			{ name: 'Picture' },
			{ name: 'Attachment' },
			{ name: 'LastModifiedDate' },
			{ name: 'CreatedDate' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            xtype: 'picgridpanel',
            store: store,
            singleSelect: true,
            region: 'center',
            title: (title),
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['add', { bttype: 'edit',
                handler: function () {
                    var rec = grid.getFirstSelection();
                    if (!rec) {
                        PICDlg.show('请选择需要操作的行。', '提示', 'alert');
                        return;
                    }

                    var edit_url = EditPageUrl;
                    var edit_style = EditWinStyle;

                    if ('PictureNews'.equals(rec.get('Type'))) {
                        edit_url = EditPageUrl_PicNews;
                        edit_style = EditWinStyle_PicNews;
                    }

                    edit_url = $.combineQueryUrl(edit_url, { type: type });
                    ExtOpenGridEditWin(grid, edit_url, "u", edit_style, null, grid.afteredit || null);
                }
            }, 'delete', '-', {
                text: '提交审批',
                hidden: true,
                iconCls: 'pic-icon-execute',
                handler: SubmitFlow
            }, {
                text: '流程跟踪',
                hidden: true,
                iconCls: 'pic-icon-execute',
                handler: FlowTrace
            }, {
                text: '发布信息',
                bttype: 'publish',
                picexecutable: true,
                iconCls: 'pic-icon-publish',
                handler: onExecBtnClick
            }, {
                text: '撤销发布',
                bttype: 'recall',
                iconCls: 'pic-icon-unpublish',
                picexecutable: true,
                handler: onExecBtnClick
            }, {
                text: '设为过期',
                bttype: 'expire',
                iconCls: 'pic-icon-time',
                picexecutable: true,
                handler: onExecBtnClick
            }, {
                text: '撤销过期',
                bttype: 'unexpire',
                iconCls: 'pic-icon-time-lapse',
                picexecutable: true,
                handler: onExecBtnClick
            }, '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '栏目名称', disabled: (!!PICState['TypeEnum'][type]), value: (PICState['TypeEnum'][type]), name: 'Type', xtype: 'piccombo', enumdata: PICState['TypeEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Type' }"} },
                { fieldLabel: '标题', id: 'Title', schopts: { qryopts: "{ mode: 'Like', field: 'Title' }"} },
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '状态', id: 'Status', xtype: 'piccombo', enumdata: PICState['StatusEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }"} },
                { fieldLabel: '发布时间', id: 'PublishDateFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'PublishDateTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'PublishDate' }"} },
                { fieldLabel: '至', id: 'PublishDateTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'PublishDateFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'PublishDate' }"} },
                { fieldLabel: '关键字', id: 'Keywords', schopts: { qryopts: "{ mode: 'Like', field: 'Keywords' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel({ singleSelect: true }),
					{ id: 'Code', header: '编号', width: 100, juncqry: true, sortable: true, dataIndex: 'Code' },
					{ id: 'Title', header: '标题', width: 250, renderer: colRender, juncqry: true, sortable: true, dataIndex: 'Title' },
					{ id: 'AuthorName', header: '作者', width: 80, juncqry: true, sortable: true, dataIndex: 'AuthorName' },
					{ id: 'Clicks', header: '点击数', width: 80, sortable: true, dataIndex: 'Clicks' },
					{ id: 'Status', header: '状态', width: 80, enumdata: PICState['StatusEnum'], sortable: true, dataIndex: 'Status', align: 'center' },
					{ id: 'IsPopup', header: '是否弹出', width: 80, enumdata: PICState['BooleanEnum'], sortable: true, dataIndex: 'IsPopup', align: 'center' },
					{ id: 'PublishDate', header: '发布时间', width: 80, renderer: ExtGridDateOnlyRender, sortable: true, dataIndex: 'PublishDate' },
					{ id: 'CreatedDate', header: '创建时间', width: 80, renderer: ExtGridDateOnlyRender, sortable: true, dataIndex: 'CreatedDate' }
                    ],
            autoExpandColumn: 'Title',
            autoExpandMin: 250
        });

        grid.on('rowclick', function (g, ridx, e) {
            var rec = this.store.getAt(ridx);

            refleshStatus(rec.get("Status"));
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }

    // 链接渲染
    function colRender(val, p, rec) {
        var rtn = val;

        switch (this.dataIndex) {
            case "Title":
                var qryurl = $.combineQueryUrl(BIZ_DATA_PAGE_URL, { path: "data.portlet.infocontent", id: rec.get('Id') });
                
                rtn = "<a class='pic-ui-link' onclick='OpenInfoViewerWin(\"" + qryurl + "\")'>" + val + "</a>";
                break;
        }

        return rtn;
    }

    function refleshStatus(status) {
        if (!status) {
            var rec = grid.getFirstSelection();

            if (rec) {
                status = rec.get("Status");
            }
        }

        var btnAdd = grid.getToolButton('add');
        var btnEdit = grid.getToolButton('edit');
        var btnDelete = grid.getToolButton('delete');
        var btnRecall = grid.getToolButton('recall');
        var btnPublish = grid.getToolButton('publish');
        var btnExpire = grid.getToolButton('expire');
        var btnUnExpire = grid.getToolButton('unexpire');

        btnUnExpire.setDisabled(true);

        if ("Published".equals(status)) {
            btnPublish.setDisabled(true);
            btnEdit.setDisabled(true);
            btnDelete.setDisabled(true);
            btnRecall.setDisabled(false);
            btnExpire.setDisabled(false);
        } else if ("Recalled".equals(status)) {
            btnPublish.setDisabled(false);
            btnEdit.setDisabled(false);
            btnDelete.setDisabled(false);
            btnRecall.setDisabled(true);
            btnExpire.setDisabled(false);
        } else if ("Expired".equals(status)) {
            btnPublish.setDisabled(true);
            btnEdit.setDisabled(false);
            btnDelete.setDisabled(false);
            btnRecall.setDisabled(true);
            btnExpire.setDisabled(true);
            btnUnExpire.setDisabled(false);
        } else {
            btnEdit.setDisabled(false);
            btnDelete.setDisabled(false);
            btnPublish.setDisabled(false);
            btnRecall.setDisabled(true);
            btnExpire.setDisabled(false);

            if ("New".equals(status)) {
                btnExpire.setDisabled(true);
            }
        }
    }

    // 提交数据成功后
    function onExecuted() {
        store.reload({ type: type });
        grid.closeeditwin();    // 关闭编辑窗口

        // 返回false, 阻止自动刷新
        return false;
    }

    function onExecBtnClick(args) {
        switch (args.bttype) {
            case "publish":
                Submit('Published');
                break;
            case "recall":
                Submit('Recalled');
                break;
            case "expire":
                Submit('Expired');
                break;
            case "unexpire":
                Submit('UnExpired');
                break;
        }
    }

            ///流程执行代码
    function SubmitFlow() {
        var smask = new Ext.LoadMask(document.body, { msg: "处理中..." });
        var recs = grid.getSelectionModel().getSelections();
        if (!recs || recs.length <= 0) {
            PICDlg.show("请先选择要提交审批的记录！");
        }
        else {
            if (recs[0].data.State == "1") {
                PICDlg.show("已审批,无需再提交!");
                return;
            }
            smask.show();
            $.ajaxExec('startflow', { Id: recs[0].data.Id, tid: "2" },
                    function (args) {
                        PICDlg.show(args.data.message);
                        onExecuted();
                        smask.hide();
                    });
        }
    }

    function Submit(val) {
        var smask = new Ext.LoadMask(document.body, { msg: "处理中..." });
        var recs = grid.getSelectionModel().getSelections();
        if (!recs || recs.length <= 0) {
            PICDlg.show("请先选择要提交的记录！");
        }
        else {
            smask.show();

            // 获取ids
            //            ExtBatchOperate('submit', recs, { status: val }, null, function (args) {
            //                PICDlg.show(args.data.message);
            //                onExecuted();
            //                smask.hide();
            //                refleshStatus(val);
            //            });

            $.ajaxExec('submit', { Id: recs[0].data.Id, status: val },
                    function (args) {
                        PICDlg.show(args.data.message);
                        onExecuted();
                        smask.hide();
                        refleshStatus(val);
                    });
        }
    }

    function FlowTrace() {
        var recs = grid.getSelectionModel().getSelections();
        if (!recs || recs.length <= 0) {
            PICDlg.show("请先选择要跟踪的记录！");
        }
        else {
            OpenWin("/WorkFlow/FlowTrace.aspx?FormId=" + recs[0].data.Id, "_blank", EditWinStyle);
        }
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>公共信息</h1></div>
</asp:Content>


