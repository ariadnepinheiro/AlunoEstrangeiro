function AddSelectedItems() {
    MoveSelectedItems(lbAvailable, lbChoosen);
    UpdateButtonState();
}
function AddAllItems() {
    MoveAllItems(lbAvailable, lbChoosen);
    UpdateButtonState();
}
function RemoveSelectedItems() {
    MoveSelectedItems(lbChoosen, lbAvailable);
    UpdateButtonState();
}
function RemoveAllItems() {
    MoveAllItems(lbChoosen, lbAvailable);
    UpdateButtonState();
}
function MoveSelectedItems(srcListBox, dstListBox) {
    srcListBox.BeginUpdate();
    dstListBox.BeginUpdate();
    var items = srcListBox.GetSelectedItems();
    for (var i = items.length - 1; i >= 0; i = i - 1) {
        dstListBox.AddItem(items[i].text, items[i].value);
        srcListBox.RemoveItem(items[i].index);
    }
    srcListBox.EndUpdate();
    dstListBox.EndUpdate();
}
function MoveAllItems(srcListBox, dstListBox) {
    srcListBox.BeginUpdate();
    var count = srcListBox.GetItemCount();
    for (var i = 0; i < count; i++) {
        var item = srcListBox.GetItem(i);
        dstListBox.AddItem(item.text, item.value);
    }
    srcListBox.EndUpdate();
    srcListBox.ClearItems();
}
function UpdateButtonState() {
    btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
    btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
    btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
    btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
}