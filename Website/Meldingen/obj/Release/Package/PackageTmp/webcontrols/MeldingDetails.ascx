<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeldingDetails.ascx.cs"
  Inherits="Meldingen.webcontrols.MeldingDetails" %>
<br />
<asp:Panel ID="PanelDetails" runat="server" Visible="false">
  <table>
    <tr>
      <td class="detailsLabel">
        nummer
      </td>
      <td class="detailsText">
        <asp:Label ID="LabelMeldingId" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;(<asp:HyperLink
          ID="HyperLinkInNewWindow" Target="_blank" runat="server" CssClass="smallLink">open in een nieuw venster</asp:HyperLink>)
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        status
      </td>
      <td class="detailsText">
        <asp:Label ID="LabelStatus" runat="server"></asp:Label>
        &nbsp; (<asp:LinkButton ID="LinkButtonChangeStatus" runat="server" OnClick="LinkButtonChangeStatus_OnClick"
          Text="status aanpassen"></asp:LinkButton>)
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        melder
      </td>
      <td class="detailsText">
        <asp:HyperLink ID="LinkMelder" runat="server" />
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        verzonden op
      </td>
      <td class="detailsText">
        <asp:Label ID="LabelVerzondenOp" runat="server" />
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        onderwerp
      </td>
      <td class="detailsText">
        <asp:Label ID="LabelOmschrijving" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        locatie
      </td>
      <td class="detailsText">
        <div id="locatieCoordinaat">
          <asp:Label ID="LabelCoordinaat" runat="server"></asp:Label>
        </div>
        <div id="locatieCoordinaatAangepast" style="display:none">
          <asp:Button ID="ButtonChangeCoordinate" runat="server" OnClick="ButtonChangeCoordinate_OnClick"
            Text="Nieuwe locatie bewaren" />
        </div>
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        foto
      </td>
      <td>
        <asp:HyperLink ID="ImageThumb" runat="server" Target="_blank" ToolTip="Klik om de foto te vergroten" />
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        e-mail
      </td>
      <td class="detailsText">
        <asp:HyperLink ID="HyperLinkMailMe" runat="server" Text="Stuur deze melding door"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        toegewezen aan
      </td>
      <td class="detailsText">
          <asp:DropDownList ID="DropDownListToeTeWijzenAan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ButtonWijsToe_Click">
          </asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td colspan="2">
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        opmerkingen
      </td>
      <td>
        <asp:TextBox ID="TextBoxNieuweOpmerking" runat="server" CssClass="nieuweOpmerkingLeeg"
          onfocus="nieuweOpmerkingSelected();" EnableViewState="false" />
        <asp:Button ID="ButtonNieuweOpmerking" runat="server" OnClick="NieuweOpmerking" Text="Voeg toe"
          Enabled="false" EnableViewState="false" />
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
      </td>
      <td>
        <div style="height: 100px; overflow: auto;">
          <asp:GridView ID="GridViewOpmerkingen" runat="server" AutoGenerateColumns="False"
            ShowHeader="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="GridViewOpmerkingen_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
              <asp:BoundField DataField="AangemaaktDoor" />
              <asp:TemplateField>
                <ItemTemplate>
                  <asp:Label ID="LabelAangemaaktOp" runat="server"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="Tekst">
                <ItemStyle Width="200px" />
              </asp:BoundField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
          </asp:GridView>
        </div>
      </td>
    </tr>
    <tr>
      <td colspan="2">
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="detailsLabel">
        bijlages
      </td>
      <td>
        <div style="height: 100px; overflow: auto;">
          <asp:GridView ID="GridViewBijlagen" runat="server" AutoGenerateColumns="False" ShowHeader="False"
            CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="GridViewBijlagen_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
              <asp:TemplateField>
                <ItemTemplate>
                  <asp:HyperLink ID="HyperlinkToonDocument" runat="server" Target="_blank"></asp:HyperLink>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
          </asp:GridView>
        </div>
      </td>
    </tr>
  </table>
</asp:Panel>
<div style="display: none">
  <asp:TextBox ID="TextboxLat" runat="server" />
  <asp:TextBox ID="TextboxLon" runat="server" />
</div>
<script type="text/javascript">

  function nieuweOpmerkingSelected() {
    var control = $("#meldingDetails_TextBoxNieuweOpmerking");
    control.select();
    if ((control.val() == "Nieuwe opmerking")) {
      control.val("");
    }
    control.keyup(function () {
      var control = $("#meldingDetails_TextBoxNieuweOpmerking");
      if ((control.val() != "") && (control.val() != "Nieuwe opmerking")) {
        control.attr("class", "nieuweOpmerkingGevuld");
        $("#meldingDetails_ButtonNieuweOpmerking").attr('disabled', false);
      }
    });
    control.blur(function () {
      var control = $("#meldingDetails_TextBoxNieuweOpmerking");
      if ((control.val() == "") || (control.val() == "Nieuwe opmerking")) {
        control.val("Nieuwe opmerking")
        control.attr("class", "nieuweOpmerkingLeeg");
        $("#meldingDetails_ButtonNieuweOpmerking").attr('disabled', true);
      }
    });
  }

  function wijzigCoordinaat(meldingId, lat, lon) {
    $("#meldingDetails_TextboxLat").val(lat);
    $("#meldingDetails_TextboxLon").val(lon);
    $("#locatieCoordinaatAangepast").show();
    $("#locatieCoordinaat").show(false);
  }

</script>
