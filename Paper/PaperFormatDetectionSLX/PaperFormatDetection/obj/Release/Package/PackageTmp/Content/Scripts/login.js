$(document).ready(function () {
        Init();
        $("#login").click(function () {
            user = $("#username").val();
            pwd = $("#password").val();
            if (Validate(user, pwd)) {
                Login(user, pwd);
            }
        });
    });
function Validate(user, pwd) {
    if (user == "" || pwd == "") {
        document.getElementById("prompt").style.display = "block";
        $("#prompt").html("电话号码或密码不能为空!");
        return false;
    }
    else return true;
}
function Login(user, pwd) {
    $.post("/User/Login", {
        username: user,
        password: pwd
    },
    function (data) {
        var result = $.parseJSON(data).status;
        document.getElementById("prompt").style.display = "block";
        if (result == 1) {
            $("#prompt").html("用户名或密码错误!")
        }
        else {
            $("#prompt").html("登录成功,正在跳转 ... ...")
            setTimeout(function () {
                window.location.href = "/Paper/Detection"
            }, 2000)
        }
    });
}
function Init() {
    $('#password').focus(function () {
        $(".space1").css({
            "background": 'url(/Resource/pwd1.png) no-repeat center center',
            "background-size": "22px 22px"
        });
    });
    $('#password').blur(function () {
        $(".space1").css({
            "background": 'url(/Resource/pwd.png) no-repeat center center',
            "background-size": "22px 22px"
        });
    });
    $('#username').focus(function () {
        $(".space").css({
            "background": 'url(/Resource/user1.png) no-repeat center center',
            "background-size": "22px 22px"
        });
    });
    $('#username').blur(function () {
        $(".space").css({
            "background": 'url(/Resource/user.png) no-repeat center center',
            "background-size": "22px 22px"
        });
    });
}