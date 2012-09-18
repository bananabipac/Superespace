package com.superespace;

import android.os.Bundle;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;

import com.google.ads.Ad;
import com.google.ads.AdListener;
import com.google.ads.AdRequest;
import com.google.ads.AdRequest.ErrorCode;
import com.google.ads.AdSize;
import com.google.ads.AdView;
import com.unity3d.player.UnityPlayerActivity;

public class SimpleAdmob extends UnityPlayerActivity implements AdListener {

	private AdView adView;
	
	private LinearLayout layout;
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		//setupAdmob();
		
	}
	
	@Override
	public void onDestroy() {
		if (adView != null) {
			adView.destroy();
		}
		super.onDestroy();
	}
	
	private void setupAdmob() {
		setupAdmobView();
		
		final String publisherID = "a1504486557cfc8";
		
		adView = new AdView(this,AdSize.BANNER,publisherID);
		adView.setAdListener(this);
		adView.loadAd(new AdRequest());
		adView.setVisibility(View.VISIBLE);
		adView.setGravity(android.view.Gravity.BOTTOM | android.view.Gravity.CENTER_VERTICAL);
		RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
	    params.addRule(RelativeLayout.ALIGN_BOTTOM, RelativeLayout.TRUE); 
	    //layout.addView(adView,params);
		layout.addView(adView,params);
	}
	
	private void setupAdmobView(){
		layout = new LinearLayout(this);
		layout.setOrientation(LinearLayout.VERTICAL);
		//layout.setGravity(android.view.Gravity.BOTTOM | android.view.Gravity.CENTER_VERTICAL);
		layout.setGravity(android.view.Gravity.BOTTOM | android.view.Gravity.CENTER_VERTICAL);
		RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
	    params.addRule(RelativeLayout.ALIGN_BOTTOM, RelativeLayout.TRUE); 
		addContentView(layout, params);
		
	}

	public void onDismissScreen(Ad arg0) {
		// TODO Auto-generated method stub
		
	}

	public void onFailedToReceiveAd(Ad arg0, ErrorCode arg1) {
		// TODO Auto-generated method stub
		
	}

	public void onLeaveApplication(Ad arg0) {
		// TODO Auto-generated method stub
		
	}

	public void onPresentScreen(Ad arg0) {
		// TODO Auto-generated method stub
		
	}

	public void onReceiveAd(Ad arg0) {
		// TODO Auto-generated method stub
		
	}
}
