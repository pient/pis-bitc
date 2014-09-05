<%@ Page Title="我的活动" Language="C#" AutoEventWireup="true" CodeBehind="MyFlowPortal.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Bpm.MyFlowPortal" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
        html, body {
            font:normal 12px verdana;
            margin:0;
            padding:0;
            border:0 none;
            overflow:hidden;
        }

        .item-outer {
            border:1px solid #bfc4e5;
            height:100%;
        }

        .item-outer ul {
            overflow: hidden;
            margin:2px;
            padding:10px;
        }

        .item-outer ul li {
            float:left;
            width: 200px;
            list-style: none;
            color: #333;
            cursor: pointer;
        }

        .item-outer.freq-item {
            height:100%;
            padding-bottom:0px;
        }

        .item-outer.freq-item ul li {
            float:none;
        }

        .item-outer-collapsed {
            background-position: 3px 3px !important;
        }

        .item-outer-header {
            background  : #bfc4e5 url(/portal/js/ext4/resources/themes/images/default/grid/group-expand-sprite.gif) no-repeat 3px -47px;
            cursor: hand;
            margin     : 1px;
            font-family : tahoma,arial,san-serif;
            font-size   : 13px;
            font-weight : bold;
            color       : #3A4B5B;
        }

        .item-inner {
            margin:5px;
        }

        .item-inner a:hover {
            color: #bfc4e5 !important;
        }

        .item-inner-start {
            color:blue;
        }
    </style>
    
    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.com.bpm.MyFlowPortal');
        }

    </script>

</asp:Content>
