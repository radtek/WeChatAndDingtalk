var userName = defaultData.email;  //这是当前绑定的用户的账号

document.getElementById("infoName").innerHTML = userName;

// 确认弹层修改
window.confirm = function (message) {
    var iframe = document.createElement("IFRAME");
    iframe.style.display = "none";
    iframe.setAttribute("src", 'data:text/plain,');
    document.documentElement.appendChild(iframe);
    var alertFrame = window.frames[0];
    var result = alertFrame.window.confirm(message);
    iframe.parentNode.removeChild(iframe);
    return result;
};

window.onscroll = function () {
    var topd = document.body.scrollTop;
    document.getElementsByClassName('tooltip-error-item')[0].style.top = topd - 1 + 'px';
    document.getElementsByClassName('tooltip-error-item')[1].style.top = topd - 1 + 'px';
    document.getElementsByClassName('tooltip-error-item')[2].style.top = topd - 1 + 'px';
    document.getElementsByClassName('tooltip-error-item')[3].style.top = topd - 1 + 'px';
    document.getElementById('tooltip-right').style.top = topd - 1 + 'px';
}
function ajax(opt) {

    opt = opt || {};
    opt.method = opt.method.toUpperCase() || 'POST';
    opt.url = opt.url || '';
    opt.async = opt.async || true;
    opt.data = opt.data || null;
    opt.success = opt.success || function () { };
    var xmlHttp = null;
    if (XMLHttpRequest) {
        xmlHttp = new XMLHttpRequest();
    }
    else {
        xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
    } var params = [];
    for (var key in opt.data) {
        params.push(key + '=' + opt.data[key]);
    }
    var postData = params.join('&');
    if (opt.method.toUpperCase() === 'POST') {
        xmlHttp.open(opt.method, opt.url, opt.async);
        xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded;charset=utf-8');
        xmlHttp.send(postData);
    }
    else if (opt.method.toUpperCase() === 'GET') {
        xmlHttp.open(opt.method, opt.url + '?' + postData, opt.async);
        xmlHttp.send(null);
    }
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            opt.success(JSON.parse(xmlHttp.responseText));
        } else if (xmlHttp.readyState == 4 && xmlHttp.status != 200) {
            opt.fail & opt.fail();
            document.getElementById('tooltip-error').style.display = 'block';
            document.getElementsByClassName('error-text')[0].innerHTML = '接口异常';
            closeRight();
        }
    };
}
// 关闭错误弹层提示
function closeError() {
    document.getElementById('tooltip-error').style.display = 'none';
    document.getElementsByClassName('error-text')[0].innerHTML = '';
}
// 关闭正确弹层提示
function closeRight() {
    document.getElementById('tooltip-right').style.display = 'none';
    document.getElementsByClassName('right-text')[0].innerHTML = '';
}

// 点击直接登录按钮
var isLogin = false;
function loginClick() {
    //这里应该是跳转页面
    if (isLogin) return;
    isLogin = true;
    //window.location.href = defaultData.redirect_url;
    window.location.href = '//cloud.hanstate.com/hanyiwealth#/DetailPage?metaObjName=HanYiWealth.PersonalInformation&listViewName=HanYiWealth.SingleObjectListView.PersonalInformationList&id=79667b3d-f58a-4ffb-b81e-fe3a2c19d704&formState=show&showBack=false&app=HanYiWealth&_k=dcnh86';
}
// 取消绑定操作
var iRelease = false;
function releaseBindClick() {
    if (iRelease) return;
    var url = defaultData.host + '/User/_UnBind';
    var redirectUrl = defaultData.host + '/Account/Bind?tenant_id=' + defaultData.tenant_id + '&inviteuser_id=' + defaultData.inviteuser_id + '&redirect_url=' + defaultData.redirect_url + '&isv_id=' + defaultData.isv_id
	//+ '&appaccount_id=' + defaultData.appaccount_id + '&type=' + defaultData.type
	+ '&batch=';
    var judgeConsole = confirm('确认解除绑定吗？');
    if (judgeConsole) {
        iRelease = true;
        ajax({
            method: 'POST',
            url: url,
            data: '',
            success: function (responseText) {
                console.log(responseText);
                if (responseText.Code !== 0) {
                    // 接口走通但是code不是0的时候
                    iRelease = false;
                    document.getElementById('tooltip-error').style.display = 'block';
                    document.getElementsByClassName('error-text')[0].innerHTML = '解除绑定失败';
                    closeRight();
                } else {
                    // 接口走通且code是0的时候
                    document.getElementsByClassName('right-text')[0].innerHTML = '解除绑定成功';
                    document.getElementById('tooltip-right').style.display = 'block';
                    closeError();
                    iRelease = true;
                    // 2秒后跳转页面
                    setTimeout(function () {
                        // 2秒后跳转到未绑定页面
                        isBind = false;
                        location.reload(true);
                        window.location.href = redirectUrl;
                    }, 2000);
                }
            },
            fail: function () {
                iRelease = false;
            }
        })
    } else {
        return
    }
}