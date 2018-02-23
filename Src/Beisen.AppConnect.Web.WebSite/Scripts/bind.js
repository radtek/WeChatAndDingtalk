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
    } else if (PageData.Type == 1 && PageData.Email) {
        //若是重新发送页面设置倒计时
        
        $('.weui_msg').eq(2).show().siblings().hide();
        $('body').css('background', '#f5f3f3');
        var sendInterval = PageData.SendInterval;
        if (sendInterval > 0) {
            $('.login_italent_content .again_send_btn').css('background', '#E4E3E3');
            $('.count_con').show();
            $('.count_down').html(sendInterval);
            countDown(sendInterval);
        }
        
        


    } else if (PageData.Type == 1 && !PageData.Email) {
        $('body').css('background', '#f5f3f3');
        $('.weui_msg').eq(1).show().siblings().hide();
    }
    $('.weui_cell_ft').bind('click',function () {
        $(this).find('img').attr('src', '../User/GetCaptchaImage?batch=' + PageData.Batch+'&r='+Math.random());
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
    //切换页面
    function changePage(index) {

        $('.weui_msg input').val('');
        $('.weui_msg').eq(index).css('display', 'block').siblings().css('display', 'none');
        $('.weui_msg').addClass('animated bounceInRight');
        $('.tz_page').css({
            display: 'none'
        });
        $('.error_message').hide().find('.error_txt').html('');
        $('.primary_content').hide();
        setTimeout(function () {
            $('.weui_msg').removeClass('bounceInRight');
            $('.weui_extra_area').css({
                display: 'block'
            });
        }, 1000);//假设这个动画持续时间为1S，然后移除掉，添加其它class
        if (index == 0) {
            $('body').css('background', '#11ABE8');
            PageData.Type = 0;
        } else {
            $('body').css('background', '#f5f3f3');
            PageData.Type = 1;
        }
        isAjax = false;
    }
    function bind(data, btnType, dataIndex) {
        
        if (btnType.isLogin) {
            if (!data.userName) {
                $('.error_message').show().find('.error_txt').html('用户名为空');
                return;
            } else if (data.captcha!=undefined && !data.captcha) {
                $('.error_message').show().find('.error_txt').html('验证码为空');
                return;
            }else{
                //var isTest = emailTest(data.userName);
                //if (!isTest) { return; }
                //$('.error_message').hide().find('.error_txt').html('');
            }
            if (!data.password) {
                $('.error_message').show().find('.error_txt').html('密码为空');
                return;
            } else {
                $('.error_message').hide().find('.error_txt').html('');
            }
        } else if (btnType.isSendEmail) {
            if (!data.userName) {
                $('.error_message').show().find('.error_txt').html('邮箱为空');
                return;
            } else {
                var isTest = emailTest(data.userName);
                if (!isTest) { return; }
                isAjax = false;
            }
        } else if (btnType.isAgainSendEmail) {

            var count_num = $('.count_con');
            if (count_num.css('display') != 'none') {
                return;
            }
            data.userName = PageData.Email;
        }
        if (!isAjax) {
            isAjax = true;
            setTimeout(function () {
                $.ajax({
                    url: '../User/_Bind',
                    type: 'post',
                    data: data
                }).done(function (resp) {
                
                    if (resp.Data.NeedCaptcha && btnType.isLogin) {
                   
                        $('.primary_content').show();
                        $('.primary_content').find('.weui_cell_ft img').attr('src', '../User/GetCaptchaImage?batch=' + PageData.Batch + '&r=' + Math.random());
                        isAjax = false;
                    }
                
                        if (resp.Code == 0) {
                            //点击登录请求成功
                            isAjax = true;
                            if (btnType.isLogin) {
                                $('.error_message').hide().find('.error_txt').html('');
                                window.location.href = resp.Data.RedirectUrl;
                            } else if (btnType.isSendEmail) {
                                PageData.Email = data.userName;
                                changePage(dataIndex);
                                $('.email_info').html(PageData.Email);
                                $('.login_italent_content .again_send_btn').css('background', '#E4E3E3');
                                $('.count_con').show().find('.count_down').html(180);
                                countDown(180);
                            
                                PageData.Type = 1;
                                $('.error_message').hide().find('.error_txt').html('');
                                isAjax = false;
                            } else if (btnType.isAgainSendEmail) {
                                $('.login_italent_content .again_send_btn').css('background', '#E4E3E3');
                                countDown(180);
                                $.alert("我们已向您的邮箱中发送了验证邮件，请注意查收。");
                            }
                        } else if (resp.Code == 3) {
                       
                            $('.error_message').show().find('.error_txt').html(resp.Message);
                            isAjax = false;
                        } else if (resp.Code == 4) {
                        
                            $('.error_message').show().find('.error_txt').html(resp.Message);
                            $('.primary_content').find('.weui_cell_ft img').attr('src', '../User/GetCaptchaImage?batch=' + PageData.Batch + '&r=' + Math.random());
                            isAjax = false;
                        } else if (resp.Code == 5) {
                            window.location.href = '../user/noaccount?type=' + PageData.Type + '&batch=' + PageData.Batch + '&email=' + data.userName+'&r='+Math.random();
                        
                        } else if (resp.Code == 6) {
                            $.alert('操作太频繁了', function () {
                                isAjax = false;
                            });
                        } else if (resp.Code == 7) {
                            window.location.href = '../user/hasbind?type=' + PageData.Type + '&batch=' + PageData.Batch + '&email=' + data.userName + '&r=' + Math.random();

                        } else if (resp.Code == 8) {
                            $.alert(resp.Message , function () {
                                window.location.href = '../user/info?r=' + Math.random();
                            });
                        
                        
                        }
               
                })
            }, 300);
        }
    }
    $('.tz_page').bind('click', function () {
        var dataIndex = $(this).attr('data-index');
        changePage(dataIndex);
    });
    $('.change_email').bind('click', function () {
        var dataIndex = $(this).attr('data-index');
        PageData.Email = '';
        changePage(dataIndex);
    });
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
        data.batch = PageData.Batch;
        data.type = PageData.Type;
        var isAgainSendEmail = $(this).hasClass('again_send_btn') ? true : false;
        var isLogin = $(this).hasClass('login_text') ? true : false;
        var isSendEmail = $(this).hasClass('send_email_btn') ? true : false;
        var isPrimary = $('.primary_content').css('display')!='none' ? true : false;
        btnType.isAgainSendEmail = isAgainSendEmail;
        btnType.isLogin = isLogin;
        btnType.isSendEmail = isSendEmail;
        var dataIndex = $(this).attr('data-index');
        data.r = Math.random();
        if (PageData.Type == 0) {
            data.userName = $.trim($('.zh_name_txt').val());
            data.password = $.trim($('.zh_password_txt').val());



        }
        if (isSendEmail) {

            data.userName = $('.txt_email').val() || '';
            
        }
        if (isAgainSendEmail) {
            data.userName = PageData.Email;
        }
        if (isPrimary) {
            data.captcha = $.trim($('.primary_content input').val()) || '';
        }
        bind(data, btnType, dataIndex);
    });
    //邮箱校验
    function emailTest(inpValue) {

        //对电子邮件的验证
        //var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
        var myreg = /^([a-zA-Z0-9][_\.\-]*)+\@([A-Za-z0-9])+((\.|-|_)[A-Za-z0-9]+)*((\.[A-Za-z0-9]{2,15}){1,2})$/;
        if (!myreg.test(inpValue)) {
            //$.alert('请输入有效的E_mail！');
            $('.error_message').show().find('.error_txt').html('请输入有效的邮箱');
            //myreg.focus();
            return false;
        } else {
            return true;
        }
    }
})