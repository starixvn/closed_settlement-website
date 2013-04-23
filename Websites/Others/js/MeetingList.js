var Meeting = {
    partnerList: null,
    partnerItem: { PartnerId: 0, PartnerName: "" },
    keyPartner: "partner._id_",
    getPartnerList: function () {
        try {
            JAjax.call({
                url: "../Services/WCF/PT/Racing/Handler.svc/GetPartnerList",
                success: function (resp) {
                    if (typeof resp != 'undefiend' && resp.length > 0) {
                        Meeting.partnerList = resp;
                        $.each(resp, function (ind, partner) {
                            var new_option = $('<option />').attr("value", ind).text(partner.PartnerName);
                            $('#partner_type').append(new_option);
                        });
                        $('#partner_type').bind('change', function (e) {
                            return Meeting.getPartnerDetail($(this).val());
                        });
                    }
                },
                isLoading: false
            });
        } catch (ex) { }
    },
    getPartnerDetail: function (indOf) {
        try {
            if (typeof (this.partnerList[indOf]) == 'undefined' || this.partnerList[indOf].PartnerId == this.partnerItem.PartnerId) { return; }
            this.partnerItem = this.partnerList[indOf];
            var _key = this.keyPartner.replace(/_id_/gi, this.partnerItem.PartnerId);
            var resp = $.jCache.getItem(_key);
            if (typeof resp == 'undefined' || resp == null) {
                try { parent.BlockUI(); } catch (ex) { }
                JAjax.call({
                    url: "../Services/WCF/PT/Racing/Handler.svc/GetPartnerDetail",
                    data: { partnerId: Meeting.partnerItem.PartnerId },
                    success: function (resp) {
                        try { parent.jQuery.unblockUI(); } catch (ex) { }
                        if (typeof resp != 'undefiend') {
                            $.jCache.setItem(_key, resp);
                            Meeting.displayPartner(resp);
                        }
                    }
                });
            } else {
                this.displayPartner(resp);
            }
        }
        catch (ex) { }
    },
    displayPartner: function (resp) {
        try {
            $("#partner_detail").show();
            $("#span_title").html('Partner / '.concat(Meeting.partnerItem.PartnerName));
            $("#partner_content").empty();
            if (typeof resp != 'undefiend' && resp.length > 0) {
                var templateRows = $("<div />").append('{{each Rows}}<div class="item {{if $index%2==0}}even{{/if}}">'
                                    + '<div class="number">${$index+1}</div>'
                                    + '<div class="meeting">${$value.MeetingName}</div>'
                                    + '<div class="currency">${$value.PoolCurrency}</div>'
                                    + '<div class="comm">${Meeting.formatComm($value.CommWin, 3)}</div>'
                                    + '<div class="comm">${Meeting.formatComm($value.CommPlace, 3)}</div>'
                                    + '<div class="comm">${Meeting.formatComm((($value.CommWin + $value.CommPlace)/2), 3)}</div>'
                                    + '<div class="comm noborder">${Meeting.formatComm($value.Fee, 2)}</div>'
                                + '</div>{{/each}}');
                templateRows.tmpl({ Rows: resp, Total: resp.length }).appendTo("#partner_content");
                $('div.item').bind('mouseover', function (evt) {
                    $('div.item').removeClass('active');
                    $(this).addClass('active');
                });
            }
            else {
                $("#partner_content").append('<div class="item"><div class="nodata">There is no information available.</div></div>');
            }
        } catch (ex) { }
    },
    formatComm: function (comm, numpre) {
        if (typeof comm == 'undefined') { var comm = 0; }
        if (typeof numpre == 'undefined') { var numpre = 4; }
        try {
            comm = comm * 100;
            comm = Math.round(comm * 1000) / 1000;//round 3 decimals
            var _commText = "".concat(comm);
            var _inof = _commText.indexOf('.');
            if (_inof != -1) {
                var _preText = _commText.substr(0, _inof + 1);
                var _lastText = _commText.substr(_inof + 1);
                var _numOdd = (_lastText.length < numpre) ? (numpre - _lastText.length) : 0;
                if (_numOdd > 0) {
                    for (var i = 0; i < _numOdd; i++) {
                        _commText = _commText.concat('0');
                    }
                }
                var result = _commText.substr(0, parseInt(numpre) + _preText.length);
                while (result.substr(result.length - 1, 1) == "0") {
                    result = result.substr(0, result.length - 1);
                }
                
                return result;
            }
        } catch (ex) { }
        return "".concat(comm);
    }
};