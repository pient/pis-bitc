<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="PubMsgView.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Msg.PubMsgView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<link href="css/styles.css" rel="stylesheet" />

<script type="text/javascript">

    function onPgLoad() {
        initPage();

        var msgObj = PICState["Message"];

        if (msgObj) {
            $('.contentTitle .mainTitle').html(msgObj.Subject);

            $('.des .depart').html("发布人： " + msgObj.FromName);
            $('.des .time').html("发布时间： " + msgObj.SentDate);

            $('#word').html(msgObj.Content);

            if (msgObj.Attachments) {
                var filePanel = Ext.create('PIC.ExtFileField', {
                    renderTo: 'attachments',
                    width: '100%',
                    readOnly: true
                });

                filePanel.setValue(msgObj.Attachments);
            }
        }
    }

    function initPage() {
        window.onscroll = function () {
            var scrollTop = document.documentElement.scrollTop;
            if (scrollTop) {
                $("#backTop").style.display = 'block';
                $("#backTop").style.top = (scrollTop + document.documentElement.clientHeight - 150) + "px";
            } else {
                $("#backTop").style.display = 'none';
            }
        }
    }

    function doZoom(size) {
        document.getElementById('word').style.fontSize = size + 'px';
    }

    function gopage(n) {
        var tag = document.getElementById("menu").getElementsByTagName("li");
        var taglength = tag.length;

        for (i = 1; i <= taglength; i++) {
            document.getElementById("m" + i).className = "";
            document.getElementById("c" + i).style.display = 'none';
        }
        document.getElementById("m" + n).className = "on";
        document.getElementById("c" + n).style.display = '';
    }

</script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">

<!-- Begin Wrapper -->
<div id="wrapper">

  <!-- Begin subHeader -->

  <div id="subHeader">
    <div class="logo"><img src="images/logo.png" /></div>
  </div>

  <!-- End subHeader -->

  
  <!-- Begin Faux Columns -->
  <div id="thiFaux">
    <!-- Begin mainContent Column -->
    <div id="mainContent">
     
     <!--内容 开始-->
     <div class="main">
       <div class="mainTop"><img src="images/mainCT.png" border="0" /></div>
       
       <div class="content">
           <div class="contentTitle">
               <div class="mainTitle"></div>
               <div class="subTitle"></div>
               <hr width="100%" size="2" color="#005baa" style="FILTER: alpha(opacity=100,finishopacity=0,style=3)" />
           </div>
           <div class="des">
               <div class="depart">发布部门：学生工作部</div>
               <div class="time">发布时间：2013-08-29 07:28</div>
               <div class="fontsize">字号：<A href="javascript:doZoom(22)">大</a> <A href="javascript:doZoom(18)">中</a> <A href="javascript:doZoom(14)">小</a></div>
           </div>
           
           <!-- word 开始 --> 
           <div id="word">&nbsp;
           </div>
           <!-- word 结束 -->

           <div id="attachments"></div>
       </div>
        
       <div class="mainFoot"><img src="images/mainCB.png" border="0" /></div>
       </div>
     <!--内容 结束-->
     
     
    </div>
    <!-- End mainContent Column -->
  </div>
  <!-- End Faux Columns -->
  
  
  <!-- Begin subFooter -->
  <div id="subFooter">
    <div class="bitcLogo"><img src="images/bitcLogo.png" /></div>
  </div>

  <!-- End subFooter -->

</div>
<!-- End Wrapper -->

<!-- Begin SiteBottom -->
<div id="backTop"><a href="#"></a></div>


</asp:Content>


