/*!
* Validation-with-lightbox v1.1
* https://github.com/soyosolution/validation-with-lightbox
* http://tool.soyosolution.com/validation-with-lightbox/
*
* Includes jQuery Library
* http://www.jquery.com/
*
* Copyright 2014-2014 Soyo Solution Company. and other contributors
* Released under the MIT license
* http://jquery.org/license
*
* Date: 2014-01-02
*/

/*================================
    For developer to config
  ================================ */
var config = {
    "form_name"          : "demo_form",                       //Your form name, not id name
    "submit_form"        : true,                              //"true" is submit form, "false" would pop-up an error message.
    "title-message"      : {
        "success_title"  :"Validation Success",               //Lightbox title when validation was success.
        "error_title"    :"Error!"                            //Lightbox title when validation was fail.
    },
    "success-message"    :"Your application is submittted.",  //Lightbox content when validation was succes.
    "error-message"      : [
        {"name"      :"firstname",                            //1st input field name (name bt not id)
         "err_msg"   :"Title 1 is empty"},                    //Related error (1st input field) if validation was incorrect.
        {"name"      :"lastname" ,                            //2st input field name
         "err_msg"   :"You have fogotten fill in Title 2"},   //Related error (2st input field) if validation was incorrect.
        {"name"      :"age",                                  //3st input field name
         "err_msg"   :"Which is the range of your age"},      //Related error (3st input field) if validation was incorrect.
        {"name"      :"gender",                               //4st input field name
         "err_msg"   :"Please select your gender"},           //Related error (4st input field) if validation was incorrect.
        {"name"      :"service"  ,                            //5st input field name
         "err_msg"   :"Which service you like most ?"},       //Related error (5st input field) if validation was incorrect.
        {"name"      :"where_from"  ,                         //6st input field name
         "err_msg"   :"You havn't fill in where to know us."} //Related error (6st input field) if validation was incorrect.         
    ],
    "footer_close_btn_text" :"Close",     //Close button tex on the bottom-right corner of pop-up message box
    "close_btn_icon"        :"X",         //Close button icon on the top-right corner of pop-up message box
};

/*================================
    Javascript Helper function
  ================================ */
Object.size = function(obj) {
    var size = 0, key;
    for (key in obj) {
        if (obj.hasOwnProperty(key)) size++;
    }
    return size;
};

/*================================
    Prepare to load data
  ================================ */
var checkItemAmount = Object.size(config["error-message"]);
var checkItem = jQuery.map(config["error-message"], function(value, index) {
    return [value];
});
//console.log(checkItem);

/*================================
    Make validation
  ================================ */
function validation_check(url){
    var msgContent ="";
    var checker = false;
    var notFalseChecker = false;
    var err_record = new Array();
    //Load input box's value
    for (var i=0; i < checkItemAmount; i++){
        //console.log("checkItem[i].name :"+checkItem[i].name);
        var value = "";
        err_record[i] = false;
        value = jQuery('#'+checkItem[i].name).val();
        //Check the input box type: text / radio, checkbox 
        if(typeof value != 'undefined'){ value = jQuery('#'+checkItem[i].name).val();}
        else{ 
            //If is checkbox or radiobox, get the value.
            if (jQuery('input[name='+checkItem[i].name+']:checked').val()){
                value = jQuery('input[name='+checkItem[i].name+']').attr("value");
                //console.log(checkItem[i].name+"checked:"+value);            
            }else{value = "";}
        }
        if (value.length > 0){ err_record[i] = true;}
        else {
            msgContent += "<li>"+checkItem[i].err_msg+"</li>";
            err_record[i] = false;
            if (i == checkItemAmount-1){}
        }
    }
    
    checker = checkForAllSame_butNotFalse(err_record);
    if ( checker == false) { displayErrorMsg(msgContent);} 
    else { 
        if(config.submit_form){
            document.forms[0].action = url;
            document.forms[config.form_name].submit();
        }else{
            displaySuccessMsg();        
        }
    }    
}//EOF of checking

function checkForAllSame_butNotFalse(arr){
    var x = arr[0];
    for(var i=1;i<arr.length;i++){
        if(x!=arr[i] || x!= true){return false;}
    }
    return true;
}

/*================================
    Display Error Message
  ================================ */
function displaySuccessMsg(){
    jQuery("#title_box").css("background", "#65B688");
    jQuery("#message_box").html('<div id="success_msg">'+config["success-message"]+'</div>');
    append_lightbox(config["title-message"].success_title, '<div id="success_msg">'+config["title-message"].success_title+'</div>');
    jQuery("#btn_close_bottom").css("background", "#65B688");
    return true;
}

function displayErrorMsg(msg){
    jQuery("#title_box").css("background", "#e69171");
    jQuery("#message_box").html(msg);
    append_lightbox(config["title-message"].error_title, "<ul>"+msg+"</ul>");
    jQuery("#btn_close_bottom").css("background", "#e69171");
    return false;
}

    function append_lightbox(title, msg){
        var msg ;
         
        /*  If the lightbox window HTML already exists in document, 
        change the img src to to match the href of whatever link was clicked
        If the lightbox window HTML doesn't exists, create it and insert it.
        (This will only happen the first time around) */
        
        if (jQuery('#lightbox').length > 0) { // #lightbox exists
             
            //place href as img src value
            jQuery('#content_box').html(msg);
            //show lightbox window - you could use .show('fast') for a transition
            jQuery('#lightbox').show();
        }
         
        else { //#lightbox does not exist - create and insert (runs 1st time only)
             
            //create HTML markup for lightbox window
            var lightbox = 
            '<div id="lightbox">' +
                '<div id="message_box_outer">'+
                '<div id="dialog_box">'+
                    '<div id="title_box"><div id="title_line">'+title+'</div><div id="btn_close_top" onclick="hide_lightbox()">'+config.close_btn_icon+'</div></div>'+
                    '<div id="content_box">' + 
                            msg+
                    '</div>' +    
                    '<div id="msg_footer"><div id="btn_close_bottom" class="lightbox_footer_close_btn" onclick="hide_lightbox()">'+config.footer_close_btn_text+'</div>'+
                '</div>' +
                '</div>' +
            '</div>';
                 
            //insert lightbox HTML into page
            jQuery('body').append(lightbox);      
        }
         
    }

/*================================
    Interactive Action
  ================================ */
jQuery( document ).ready(function() {
    $('body').on('click','#lightbox, #message_box_outer', function () {
        hide_lightbox();
    });    
});

function hide_lightbox() {
    jQuery('#lightbox').css("display","none");
}

