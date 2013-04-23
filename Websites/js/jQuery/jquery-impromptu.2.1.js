/*
 * jQuery Impromptu
 * By: Trent Richardson [http://trentrichardson.com]
 * Version 2.1
 * Last Modified: 2/16/2009
 * 
 * Copyright 2009 Trent Richardson
 * Dual licensed under the MIT and GPL licenses.
 * http://trentrichardson.com/Impromptu/GPL-LICENSE.txt
 * http://trentrichardson.com/Impromptu/MIT-LICENSE.txt
 * 
 */
 
jQuery.extend({	
	ImpromptuDefaults: { prefix:'jqi', buttons:{ Ok:true }, loaded:function(){}, submit:function(){return true;}, callback:function(){}, opacity:0.6, zIndex: 999, overlayspeed:'slow', promptspeed:'fast', show:'fadeIn', focus:0, useiframe:false, top: '100px', persistent: true },
	ImpromptuStateDefaults: { html: '', buttons: { Ok:true }, focus: 0, submit: function(){return true;} },
	SetImpromptuDefaults: function(o){ 
		jQuery.ImpromptuDefaults = jQuery.extend({},jQuery.ImpromptuDefaults,o);
	},
	SetImpromptuStateDefaults: function(o){ 
		jQuery.ImpromptuStateDefaults = jQuery.extend({},jQuery.ImpromptuStateDefaults,o);
	},
	ImpromptuGoToState: function(state){
		jQuery('.'+ jQuery.ImpromptuCurrentPrefix +'_state').slideUp('slow');
		jQuery('#'+ jQuery.ImpromptuCurrentPrefix +'_state_'+ state).slideDown('slow',function(){
			jQuery(this).find('.'+ jQuery.ImpromptuCurrentPrefix +'defaultbutton').focus();
		});
	},
	ImpromptuClose: function(){
		jQuery('#'+ jQuery.ImpromptuCurrentPrefix +'box').fadeOut('fast',function(){ jQuery(this).remove(); });
	},
	ImpromptuCurrentPrefix: 'jqi',
	prompt: function(m,o){
		o = jQuery.extend({},jQuery.ImpromptuDefaults,o);
		jQuery.ImpromptuCurrentPrefix = o.prefix;
		
		var ie6 = (jQuery.browser.msie && jQuery.browser.version < 7);	
		var b = jQuery(document.body);
		var w = jQuery(window);
				
		//build the box and fade
		var msgbox = '<div class="'+ o.prefix +'box" id="'+ o.prefix +'box">';		
		if(o.useiframe && ((jQuery.browser.msie && jQuery('object, applet').length > 0) || ie6))//if you want to use the iframe uncomment these 3 lines
			msgbox += '<iframe src="javascript:;" class="'+ o.prefix +'fade" id="'+ o.prefix +'fade"></iframe>';
		else{ 
			if(ie6) jQuery('select').css('visibility','hidden');
			msgbox +='<div class="'+ o.prefix +'fade" id="'+ o.prefix +'fade"></div>';
		}	
		msgbox += '<div class="'+ o.prefix +'" id="'+ o.prefix +'"><div class="'+ o.prefix +'container"><div class="'+ o.prefix +'close">X</div><div id="'+ o.prefix +'states"></div>';		
		msgbox += '</div></div></div>';
		
		var jqib =b.append(msgbox).children('#'+ o.prefix +'box');
		var jqi = jqib.children('#'+ o.prefix);
		var jqif = jqib.children('#'+ o.prefix +'fade');
		
		//if a string was passed, convert to a single state
		if(m.constructor == String){
			m = { state0: { html: m , buttons: o.buttons, focus: o.focus, submit: o.submit } };
		}
		
		//build the states
		var states = "";
		
		jQuery.each(m,function(statename,stateobj){
			stateobj = jQuery.extend({},jQuery.ImpromptuStateDefaults,stateobj);
			m[statename] = stateobj;
			
			states += '<div id="'+ o.prefix +'_state_'+ statename +'" class="'+ o.prefix +'_state" style="display:none;"><div class="'+ o.prefix +'message">'+ stateobj.html +'</div><div class="'+ o.prefix +'buttons">';
			jQuery.each(stateobj.buttons,function(k,v){ 
				states += '<button name="'+ o.prefix +'_'+ statename +'_button'+ k +'" id="'+ o.prefix +'_'+ statename +'_button'+ k +'" value="'+ v +'">'+ k +'</button>'
			});
			states += '</div></div>';
		});		
		
		//insert the states...
		jqi.find('#'+ o.prefix +'states').html(states).children('.'+ o.prefix +'_state:first').css('display','block');
		
		//Events
		jQuery.each(m,function(statename,stateobj){
			var state = jqi.find('#'+ o.prefix +'_state_'+ statename);
			
			state.children('.'+ o.prefix +'buttons').children('button').click(function(){ 
				var msg = state.children('.'+ o.prefix +'message');
				var clicked = stateobj.buttons[jQuery(this).text()];	
				if(stateobj.submit(clicked,msg))				
					removePrompt(true,clicked,msg);
			});
			state.find('.'+ o.prefix +'buttons button:eq('+ stateobj.focus +')').addClass(o.prefix +'defaultbutton');
			
		});

		var getWindowScrollOffset = function(){ 
			return (document.documentElement.scrollTop || document.body.scrollTop) + 'px'; 
		};		
		
		var getWindowSize = function(){ 
			var size = {
				width: window.innerWidth || (window.document.documentElement.clientWidth || window.document.body.clientWidth),
				height: window.innerHeight || (window.document.documentElement.clientHeight || window.document.body.clientHeight)
			};
			return size;
		};
		
		var ie6scroll = function(){ 
			jqib.css({ top: getWindowScrollOffset() }); 
		};
		
		var fadeClicked = function(){
			if(o.persistent){
				var i = 0;
				jqib.addClass(o.prefix +'warning');
				var intervalid = setInterval(function(){ 
					jqib.toggleClass(o.prefix +'warning');
					if(i++ > 1){
						clearInterval(intervalid);
						jqib.removeClass(o.prefix +'warning');
					}
				}, 100);
			}
			else removePrompt();
		};		
		

		var escapeKeyClosePrompt = function(e){
			var key = (window.event) ? event.keyCode : e.keyCode; // MSIE or Firefox?
			if(key==27) removePrompt();
		};

		var positionPrompt = function(){
			var wsize = getWindowSize();
			jqib.css({ position: (ie6)? "absolute" : "fixed", height: wsize.height, width: "100%", top: (ie6)? getWindowScrollOffset():0, left: 0, right: 0, bottom: 0 });
			jqif.css({ position: "absolute", height: wsize.height, width: "100%", top: 0, left: 0, right: 0, bottom: 0 });
			jqi.css({ position: "absolute", top: o.top, left: "50%", marginLeft: ((((jqi.css("paddingLeft").split("px")[0]*1) + jqi.width())/2)*-1) });					
		};
		
		var stylePrompt = function(){
			jqif.css({ zIndex: o.zIndex, display: "none", opacity: o.opacity });
			jqi.css({ zIndex: o.zIndex+1, display: "none" });
			jqib.css({ zIndex: o.zIndex });
		}
		
		var removePrompt = function(callCallback, clicked, msg){
			jqi.remove(); 
			if(ie6)b.unbind('scroll',ie6scroll);//ie6, remove the scroll event
			w.unbind('resize',positionPrompt);			
			jqif.fadeOut(o.overlayspeed,function(){
				jqif.unbind('click',fadeClicked);
				jqif.remove();
				if(callCallback) o.callback(clicked,msg);
				jqib.unbind('keypress',escapeKeyClosePrompt);
				jqib.remove();
				if(ie6 && !o.useiframe) jQuery('select').css('visibility','visible');
			});
		}
		
		positionPrompt();
		stylePrompt();	

		if(ie6) w.scroll(ie6scroll);//ie6, add a scroll event to fix position:fixed
		jqif.click(fadeClicked);
		w.resize(positionPrompt);
		jqib.keypress(escapeKeyClosePrompt);
		jqi.find('.'+ o.prefix +'close').click(removePrompt);
		
		//Show it
		jqif.fadeIn(o.overlayspeed);
		jqi[o.show](o.promptspeed,o.loaded);
		jqi.find('#'+ o.prefix +'states .'+ o.prefix +'_state:first .'+ o.prefix +'defaultbutton').focus();
		
		return jqib;
	}	
});
