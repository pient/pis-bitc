
<%@ Page Title="职称" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeTitleList.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeTitleList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=400,scrollbars=yes");
    var EditPageUrl = "HR_EmployeeTitleEdit.aspx";

    var store, grid, viewport;
    var eid;

    function onPgLoad() {
        eid = $.getQueryString({ "ID": "eid" });
        EditPageUrl = $.combineQueryUrl(EditPageUrl, { eid: eid });

        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'HR_EmployeeTitleList',
            idProperty: 'Id',
            loadargs: { eid: eid },
            fields: [
			{ name: 'Id' },
			{ name: 'EmployeeId' },
			{ name: 'Title' },
			{ name: 'CertiNumber' },
			{ name: 'InauguralDate' },
			{ name: 'LanguageScore' },
			{ name: 'HoldDate' },
			{ name: 'ApplyStatus' },
			{ name: 'Attachments' },
			{ name: 'Memo' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' },
			{ name: 'CreatedDate' },
			{ name: 'LastModifiedDate' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            xtype: 'picgridpanel',
            store: store,
            region: 'center',
            title: '职称',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['add', 'edit', 'delete', '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Title', dataIndex: 'Title', header: '职称', renderer: colRender, width: 100, sortable: true },
					{ id: 'CertiNumber', dataIndex: 'CertiNumber', header: '证书编号', width: 100, sortable: true },
					{ id: 'InauguralDate', dataIndex: 'InauguralDate', header: ' 任职日期', width: 100, sortable: true },
					{ id: 'HoldDate', dataIndex: 'HoldDate', header: '取得时间', width: 100, sortable: true }
                    ],
            autoExpandColumn: 'Title'
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
                return ExtGridRenderLink(grid, PICState['TitleEnum'][val], { url: EditPageUrl, style: EditWinStyle })
                break;
        }

        return rtn;
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>职称</h1></div>
</asp:Content>


