
<%@ Page Title="从业资质" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeJobTitleList.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeJobTitleList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
    var EditPageUrl = "HR_EmployeeJobTitleEdit.aspx";

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
            dsname: 'HR_EmployeeJobTitleList',
            idProperty: 'Id',
            loadargs: { eid: eid },
            fields: [
			{ name: 'Id' },
			{ name: 'EmployeeId' },
			{ name: 'Name' },
			{ name: 'Number' },
			{ name: 'HoldDate' },
			{ name: 'HoldWay' },
			{ name: 'ValidityDate' },
			{ name: 'AuditDate' },
			{ name: 'RegisterDate' },
			{ name: 'RegisterPlace' },
			{ name: 'Picture' },
			{ name: 'Attachments' },
			{ name: 'Memo' },
			{ name: 'ApplyStatus' },
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
            title: '从业资质',
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
					{ id: 'Name', dataIndex: 'Name', header: '名称', renderer: colRender, width: 120, sortable: true },
					{ id: 'Number', dataIndex: 'Number', header: '证书号', width: 100, sortable: true },
					{ id: 'HoldDate', dataIndex: 'HoldDate', header: '取资日期', width: 100, sortable: true },
					{ id: 'HoldWay', dataIndex: 'HoldWay', header: '取资方式', width: 100, sortable: true },
					{ id: 'ValidityDate', dataIndex: 'ValidityDate', header: '有效期', width: 100, sortable: true },
					{ id: 'AuditDate', dataIndex: 'AuditDate', header: '评审日期', width: 100, sortable: true },
					{ id: 'RegisterDate', dataIndex: 'RegisterDate', header: '注册日期', width: 100, sortable: true },
					{ id: 'RegisterPlace', dataIndex: 'RegisterPlace', header: '发证机构', width: 100, sortable: true }
                    ]
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
            case "Name":
                return ExtGridRenderLink(grid, PICState['TitleEnum'][val], { url: EditPageUrl, style: EditWinStyle })
                break;
        }

        return rtn;
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>从业资质</h1></div>
</asp:Content>


