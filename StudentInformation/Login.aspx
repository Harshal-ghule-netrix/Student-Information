<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="StudentInformation.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
</head>
<body>
    <div class="row">
        <div class="col-md-6 col-md-offset-3">
            <form id="form1" name="LoginForm" runat="server">
                <div class="contact-form" style="max-width: 500px; margin-left: 50px;">
                    <h1 class="form-signin-heading">Login </h1>
                    <div>
                        <asp:Label runat="server" Text="Username" Font-Bold="true" />
                        <asp:RequiredFieldValidator ID="UsernameValidator" runat="server" ControlToValidate="txtUsername" ErrorMessage="* User Name Required" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtUsername" name="txtUsername" placeholder="Enter Username" />
                    </div>
                    <div>
                        <asp:Label runat="server" Text="Password" Font-Bold="true" />
                        <asp:RequiredFieldValidator ID="PasswordValidator" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Password Required" ForeColor="#FF3300" />
                        <asp:RegularExpressionValidator ID="PasswordRegValidator" runat="server" ControlToValidate="txtPassword" ValidationExpression="^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$" ErrorMessage="* minimun length of 8 caracters and maximun of 16 at least one digit, lower case and one uppper case" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword" name="txtPassword" TextMode="Password" placeholder="Enter Password" />
                    </div>
                    <hr />
                    <div class="btn-toolbar">
                        <asp:Button Class="btn btn-primary" runat="server" Text="Login" OnClick="onLogin" Style="width: 69px" />
                        <a href="RegistrationForm.aspx">Signup</a>
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>

</html>
