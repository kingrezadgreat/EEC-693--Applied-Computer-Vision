   M S B A M L   `   `   `  ����N  Jlecture_6_depth_app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:lecture_6_depth_applecture_6_depth_app  5   	   $    lecture_6_depth_app.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:lecture_6_depth_app     5   	   -   5          � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ image1��5
      $
��Left=�6    $	��240��6;   $��
115,92,0,0q�6H   $	��Top=�6\   $	��320��6t   6�   �� 5   
   $��	Max Depth��6   $
��Left=�6$   $��
496,92,0,0q�6?   $	��Top=�6S   �� 5   
   $��	Min Depth��6   $
��Left=�6$   $��496,118,0,0q�6?   $	��Top=�6T   �� 5   
   -   5         ��Name$ maxDepthField��5      $
��Left=�6)   $��23��6D   $��
579,93,0,0q�6P    �� TextWrapping$
 Wrap=�6d   $��TextBox��6x   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $ minDepthField��5      $
��Left=�6)   $��23��6D   $��579,119,0,0q�6P   $
 Wrap=�6e   $��TextBox��6y   $	��Top=�6�   $	��120��6�   �� 5   
   $��X:��6   $
��Left=�6   $��496,207,0,0q�68   $	��Top=�6M   �� 5   
   -   5        $ pixelXField��5      $
��Left=�6'   $��23��6B   $��579,208,0,0q�6N   $
 Wrap=�6c   $��TextBox��6w   $	��Top=�6�   $	��120��6�   �� 5   
   $��Y:��6   $
��Left=�6   $��496,233,0,0q�68   $	��Top=�6M   �� 5   
   -   5        $ pixelYField��5      $
��Left=�6'   $��23��6B   $��579,234,0,0q�6N   $
 Wrap=�6c   $��TextBox��6w   $	��Top=�6�   $	��120��6�   �� 5   
   $��
DepthIndex��6   $
��Left=�6%   $��496,261,0,0q�6@   $	��Top=�6U   �� 5   
   -   5        $ depthIndexField��5      $
��Left=�6+   $��23��6F   $��579,262,0,0q�6R   $
 Wrap=�6g   $��TextBox��6{   $	��Top=�6�   $	��120��6�   �� 5   
   $��	Depth(mm)��6   $
��Left=�6$   $��496,287,0,0q�6?   $	��Top=�6T   �� 5   
   -   5        $ 
depthField��5      $
��Left=�6&   $��23��6A   $��579,288,0,0q�6M   $
 Wrap=�6b   $��TextBox��6v   $	��Top=�6�   $	��120��6�   5      5      