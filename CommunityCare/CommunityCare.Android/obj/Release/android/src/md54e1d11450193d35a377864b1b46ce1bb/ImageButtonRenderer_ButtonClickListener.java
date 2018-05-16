package md54e1d11450193d35a377864b1b46ce1bb;


public class ImageButtonRenderer_ButtonClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("ImageButton.Android.ImageButtonRenderer+ButtonClickListener, ImageButton.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ImageButtonRenderer_ButtonClickListener.class, __md_methods);
	}


	public ImageButtonRenderer_ButtonClickListener ()
	{
		super ();
		if (getClass () == ImageButtonRenderer_ButtonClickListener.class)
			mono.android.TypeManager.Activate ("ImageButton.Android.ImageButtonRenderer+ButtonClickListener, ImageButton.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
