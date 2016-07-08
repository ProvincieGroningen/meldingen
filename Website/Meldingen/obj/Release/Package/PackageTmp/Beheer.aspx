<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Beheer.aspx.cs" Inherits="Meldingen.Beheer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Beheer Meldingen</title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="beheerTable">
                <tr>
                    <td><span class="withTooltip" title="Lijst van items voor het veld 'Toewijzen aan'. Een item per regel.">Toe te wijzen aan:</span></td>
                    <td>
                        <asp:TextBox ID="TextBoxToeTeWijzenAan" runat="server" TextMode="MultiLine" Height="150px" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><span class="withTooltip" title="Een geldig emailadres waar het maandrapport naar moet worden verstuurd">Email adres maandrapport:</span></td>
                    <td>
                        <asp:TextBox ID="TextBoxMailadresMaandrapport" runat="server" Width="300px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorMail" runat="server" ErrorMessage="Vul een geldig emailadres in" ControlToValidate="TextBoxMailadresMaandrapport" CssClass="validator" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td><span class="withTooltip" title="De begeleidende tekst bij het emailbericht met het maandrapport">Emailbericht maandrapport:</span></td>
                    <td>
                        <asp:TextBox ID="TextBoxMailBodyMaandrapport" runat="server" Height="150px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="vertical-align: middle">
                        <asp:Button ID="ButtonBewaar" runat="server" Text="Bewaar" OnClick="ButtonBewaar_Click" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="ButtonVerstuurMaandrapport" runat="server" Text="Verstuur maandrapport" OnClick="ButtonVerstuurMaandrapport_Click" OnClientClick="hide(this);" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><a href=".">terug</a></td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
