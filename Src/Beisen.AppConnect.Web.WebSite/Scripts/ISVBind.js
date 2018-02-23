$(function () {
    $('body').css('background', '#11ABE8');
    //消除ios下click的300毫秒的延迟
    FastClick.attach(document.body);
    //解决重复提交问题
    var isAjax = false;
    //根据页面中type的值显示页面内容
    if (this.timer2) {
        clearInterval(this.timer2);
    }
    if (PageData.Type == 0) {
        $('.weui_msg').eq(0).show().siblings().hide();
        $('body').css('background', '#11ABE8');
    }
    //验证码
    $('.weui_cell_ft').bind('click', function () {
        $(this).find('img').attr('src', '../User/GetCaptchaImage?batch=' + PageData.Batch + '&r=' + Math.random());
    });
    function countDown(num) {
        var timeNum = num;
        if (this.timer2) {
            clearInterval(this.timer2);
        }
        this.timer2 = setInterval(function () {
            if (timeNum < 0) {
                clearInterval(this.timer2);
                $('.count_con').find('.count_down').html('');
                $('.count_con').hide();
                $('.login_italent_content .again_send_btn').css('background', '#11ABE8');
            } else {
                $('.login_italent_content .again_send_btn').css('background', '#E4E3E3');
                $('.count_con').show().find('.count_down').html(timeNum);
                timeNum--;
            }

        }, 1000);
    }
    function bind(data, btnType, dataIndex) {
        if (btnType.isLogin) {
            if (!data.userName) {
                $('.error_message').show().find('.error_txt').html('用户名为空');
                return;
            }
            if (!data.password) {
                $('.error_message').show().find('.error_txt').html('密码为空');
                return;
            } else {
                $('.error_message').hide().find('.error_txt').html('');
            }
        }
        if (!isAjax) {
            isAjax = true;
            setTimeout(function () {
                $.ajax({
                    url: '../ISV/LoginAuthorize',
                    type: 'post',
                    data: data
                }).done(function (resp) {
                    if (resp.ErrCode == 1) {
                        isAjax = true;
                        if (btnType.isLogin) {
                            $('.error_message').hide().find('.error_txt').html('');
                            var redirectUrl = resp.AuthRedirectUrl;
                            window.location.href = redirectUrl;
                        }
                    } else {
                        $.alert(resp.ErrMsg, function () {
                            window.location.href = '../ISV/LoginPage?r=' + Math.random();
                        });
                    }
                })
            }, 300);
        }
    }
    $('.login_italent_content .bind_btn').bind('click', function () {
        var nowTime = new Date().getTime();
        var clickTime = $(this).attr('data-ctime');
        if ((clickTime != '' || clickTime != 'undefineds') && (nowTime - clickTime < 5000)) {
            return false;
        } else {
            $(this).attr('data-ctime', nowTime);
        }
        var data = {};
        var btnType = {};
        data.type = PageData.Type;
        var isAgainSendEmail = $(this).hasClass('again_send_btn') ? true : false;
        var isLogin = $(this).hasClass('login_text') ? true : false;
        var isSendEmail = $(this).hasClass('send_email_btn') ? true : false;
        var isPrimary = $('.primary_content').css('display') != 'none' ? true : false;
        btnType.isAgainSendEmail = isAgainSendEmail;
        btnType.isLogin = isLogin;
        btnType.isSendEmail = isSendEmail;
        var dataIndex = $(this).attr('data-index');
        data.r = Math.random();
        if (PageData.Type == 0) {
            data.userName = $.trim($('.zh_name_txt').val());
            data.password = $.trim($('.zh_password_txt').val());
        }
        if (isPrimary) {
            data.captcha = $.trim($('.primary_content input').val()) || '';
        }
        bind(data, btnType, dataIndex);
    });
})