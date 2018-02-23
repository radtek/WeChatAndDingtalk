
var isNAme = false; //名是否验证正确
var isPhone = false; //手机是否验证正确
var isPassword = false;//密码是否验证正确
var isVertification = false; //验证码是否验证正确


var isEye = false;//点击切换眼睛的状态
var isClick = false;//禁止注册重复点击的数据

window.onscroll = function () {
    var topd = document.body.scrollTop;
    document.getElementsByClassName('tooltip-error-item')[0].style.top = topd - 1 + 'px';
    document.getElementsByClassName('tooltip-error-item')[1].style.top = topd - 1 + 'px';
    document.getElementsByClassName('tooltip-error-item')[2].style.top = topd - 1 + 'px';
    document.getElementsByClassName('tooltip-error-item')[3].style.top = topd - 1 + 'px';
    document.getElementById('tooltip-right').style.top = topd - 1 + 'px';
}
// 接口方法
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
        }
        else if (xmlHttp.readyState == 4 && xmlHttp.status != 200) {
            document.getElementsByClassName('tooltip-error4')[0].style.display = 'block';
            document.getElementsByClassName('error-text4')[0].innerHTML = '接口异常';
            closeRight();
        }
    };
}

// 关闭错误弹层提示
function closeError(i) {
    var classNameP = 'tooltip-error' + i;
    var classNameT = 'error-text' + i;
    document.getElementsByClassName(classNameP)[0].style.display = 'none';
    document.getElementsByClassName(classNameT)[0].innerHTML = '';
    document.getElementsByClassName('tooltip-error4')[0].style.display = 'none';
    document.getElementsByClassName('error-text4')[0].innerHTML = '';
}

// 关闭正确弹层提示
function closeRight() {
    document.getElementById('tooltip-right').style.display = 'none';
    document.getElementsByClassName('right-text')[0].innerHTML = '';
}

// 验证注册按钮是否可以点击
function isRegistered() {
    if (!isNAme || !isPhone || !isPassword || !isVertification) {
        document.getElementById('buttonCover').style.display = 'block';
        document.getElementById('registeredButton').style.backgroundColor = '#7ECAFA';
    } else {
        document.getElementById('buttonCover').style.display = 'none';
        document.getElementById('registeredButton').style.backgroundColor = '#0090F4';
    }
}

// 验证用户名
function testUserName(nameInput) {
    // 这是判断用户名称的方法，目前名字还没有规则
    var value = nameInput.value;
    if (value === '') {
        isNAme = false
    } else {
        isNAme = true
    }
    isRegistered();
}

// 验证密码
function testPassword(passwordInput) {
    // 这里是判断密码的格式，目前没有密码格式的验证规则
    var value = passwordInput.value;
    if (value === '') {
        isPassword = false;
        isRegistered();
        return
    }
    if (value.length < 6 || value.length > 20) {
        isPassword = false
        document.getElementsByClassName('error-text2')[0].innerHTML = '密码格式不正确，应为6-20位字符';
        closeRight();
        document.getElementsByClassName('tooltip-error2')[0].style.display = 'block';
    } else {
        isPassword = true;
        closeError(2);
    }
    isRegistered();
}
function changePassword1(input1) {
    var value = input1.value;
    document.getElementsByClassName('input-password-pw')[0].value = value;
    document.getElementsByClassName('input-password-te')[0].value = value;
}
function changePassword2(input2) {
    var value = input2.value;
    document.getElementsByClassName('input-password-pw')[0].value = value;
}

// 点击眼睛，密码可见或不可见

function clickEye(eye) {
    if (isEye === false) {
        var value = document.getElementsByClassName('input-password-pw')[0].value;
        document.getElementsByClassName('input-password-pw')[0].style.display = 'none';
        document.getElementsByClassName('input-password-te')[0].style.display = 'inline-block';
        document.getElementsByClassName('input-password-te')[0].value = value;
        eye.className = 'password-eye open-eye';
        document.getElementsByClassName('input-password-te')[0].blur();
        document.getElementsByClassName('input-password-pw')[0].blur();
        isEye = true;
    } else {
        var value = document.getElementsByClassName('input-password-te')[0].value;
        document.getElementsByClassName('input-password-te')[0].style.display = 'none';
        document.getElementsByClassName('input-password-pw')[0].style.display = 'inline-block';
        document.getElementsByClassName('input-password-pw')[0].value = value;
        eye.className = 'password-eye close-eye';
        document.getElementsByClassName('input-password-te')[0].blur();
        document.getElementsByClassName('input-password-pw')[0].blur();
        isEye = false;
    }
}
// ----------------------------验证密码结束-------------------------------------------

//验证手机号码的格式以及是否注册过
function testPhoneNumber(input) {
    var mobile_number = input.value;
    if (!mobile_number) {
        isPhone = false;
        isRegistered();
        return
    } else {
        var mobile_rule = /^1[34578]\d{9}$/;
        if (!mobile_rule.test(mobile_number) || mobile_number.length > 11) {
            document.getElementsByClassName('error-text1')[0].innerHTML = '手机号格式不正确';
            closeRight();
            document.getElementsByClassName('tooltip-error1')[0].style.display = 'block';
            isPhone = false;
            isRegistered();
            return;
        } else {
            //拿到手机号走接口
            // closeError(1);
            postMobileNumber(mobile_number);
        }
    }
}
// 验证手机号是否注册过
function postMobileNumber(mNumber) {
    var url = defaultData.host + '/Account/CheckMobile?mobile=' + mNumber;

    ajax(
        {
            method: 'POST',
            url: url,
            data: '',
            success: function (responseText) {
                if (responseText.Code !== 1) {
                    // 接口走通但是code不是1的时候
                    console.log('该手机号已注册');
                    document.getElementsByClassName('error-text1')[0].innerHTML = '该手机号已注册';
                    closeRight();
                    document.getElementsByClassName('tooltip-error1')[0].style.display = 'block';
                    // 修改手机验证码状态，判断注册是否可用            
                    isPhone = false;
                    isRegistered();
                } else {
                    // 接口走通且code是1的时候
                    closeError(1);
                    // 修改手机验证码状态，判断注册是否可用                        
                    isPhone = true;
                    isRegistered();
                    document.getElementsByClassName('input-verification-tooltip')[0].className = 'input-verification-tooltip';
                }
            }
        }
    )




}

// 获取验证码以及再次获取验证码
function getVerificationCode(button) {
    if (!isPhone) return;
    if (countdown !== 90) return;
    getVerificationTime(button);
    document.getElementsByClassName('input-verification-text').value = '';
    const mobileNumber = document.getElementsByClassName('input-mobile-number-text')[0].value;  //手机号

    // 获取验证码的url
    var url = defaultData.host + '/Account/SendISVMobileValCode?tenant_id=' + defaultData.tenant_id + '&mobile=' + mobileNumber + '&isv_id=' + defaultData.isv_id;
    ajax(
           {
               method: 'POST',
               url: url,
               data: '',
               success: function (responseText) {
                   console.log(responseText);
                   if (responseText.Code !== 1) {
                   } else {
                       // 接口走通且code是1的时候                       
                   }
               }
           }
       )
}
// 计时器
var countdown = 90;
function getVerificationTime(button) {

    if (countdown === 0) {
        button.innerHTML = '再次获取';
        button.style.color = '#1e80c7';
        countdown = 90;
    } else {
        button.innerHTML = '再次获取(' + countdown + 's)';
        button.style.color = '#bfc4c7';
        countdown--;
        setTimeout(function () {
            getVerificationTime(button);
        }, 1000);
    }

}

//验证验证码是否正确、有效
function verficationCodeTest(input) {

    var code = input.value;
    const mobileNumber = document.getElementsByClassName('input-mobile-number-text')[0].value;  //手机号
    if (!code || code.length < 6) {
        isVertification = false;
        isRegistered();
        return;
    } else {
        // 接口路径
        var url = defaultData.host + '/Account/VerifyCode?code=' + code + '&mobile=' + mobileNumber;
        var data = {};

        // } catch (error) {
        //     console.log(error);
        // }
        ajax(
           {
               method: 'POST',
               url: url,
               data: '',
               success: function (responseText) {
                   console.log(responseText);
                   if (responseText.Code !== 1) {
                       // 接口走通但是code不是1的时候
                       console.log('验证码错误、验证码已经过期');
                       document.getElementsByClassName('error-text3')[0].innerHTML = responseText.Message;
                       closeRight();
                       document.getElementsByClassName('tooltip-error3')[0].style.display = 'block';
                       // 修改验证验证码状态，判断注册是否可用                
                       isVertification = false;
                       isRegistered();
                   } else {
                       // 接口走通且code是1的时候
                       closeError(3);
                       // 修改验证验证码状态，判断注册是否可用
                       isVertification = true;
                       isRegistered();

                   }
               }
           }
       )

    }

}
// 点击注册
function registeredClick() {
    var name = document.getElementsByClassName('input-name-text')[0].value; //注册名
    var number = document.getElementsByClassName('input-mobile-number-text')[0].value; //手机号
    var password = document.getElementsByClassName('input-password-text')[0].value; //密码
    var verficationCode = document.getElementsByClassName('input-verification-text')[0].value; //验证码
    if (isClick) return;
    //点击走注册接口
    postRegistered(name, number, password, verficationCode);

}
// 注册走接口
function postRegistered(name, number, password, verficationCode) {
    var url = defaultData.host + '/Account/Register?tenant_id=' + defaultData.tenant_id + '&mobile=' + number + '&code=' + verficationCode + '&username=' + name + '&password=' + password + '&inviteuser_id=' + defaultData.inviteuser_id + '&batch=' + defaultData.batch + '&redirect_url=' + defaultData.redirect_url;
    var data = {};
    isClick = true;

    ajax({
        method: 'POST',
        url: url,
        data: '',
        success: function (responseText) {
            if (responseText.Code !== 1) {
                // 接口走通但是code不是1的时候
                document.getElementsByClassName('error-text4')[0].innerHTML = '注册失败';
                closeRight();
                document.getElementsByClassName('tooltip-error4')[0].style.display = 'block';
                isClick = false;
            } else {
                // 接口走通且code是1的时候
                document.getElementsByClassName('right-text')[0].innerHTML = '注册成功';
                closeError(4);
                document.getElementById('tooltip-right').style.display = 'block';
                isClick = true;
                setTimeout(function () {
                    // 2秒后执行跳转页面的方法
                    window.location.href = defaultData.redirect_url;
                }, 2000);
            }
        }
    })
}
