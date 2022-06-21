<?php

namespace App\Http\Controllers;

use App\Models\Entry;
use App\Models\Identifier;
use App\Models\Record;
use App\Models\Source;
use App\Models\Time;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;

class RecordController extends Controller
{
    /**
     * Store a newly created resource in storage.
     *
     * @param Source $source
     * @param Request $request
     * @return RedirectResponse
     */
    public function store(Source $source, Request $request): RedirectResponse
    {
        $record = Record::store($source);

        return redirect()->route('sources.records.show', [$source, $record]);
    }

    /**
     * Display the specified resource.
     *
     * @param Source $source
     * @param Record $record
     * @param Request $request
     * @return Application|Factory|View
     */
    public function show(Source $source, Record $record, Request $request): View|Factory|Application
    {
        $start_time = Entry::query()->where('record_id', $record->id)->latest()->first()->end ?? null;
        $embed_time = $start_time != null ? Time::toSeconds($start_time) : 0;

        return view('sources.records', [
            'source'        => $source,
            'record'        => $record,
            'entries'       => $record->entries()->get(),
            'identifier'    => Identifier::infer(),
            'can_vote'      => Identifier::infer() != $record->identifier,
            'start_time'    => $start_time,
            'embed_time'    => $embed_time,
            'advanced_mode' => $request->input('mode') == 'advanced',
            'export'        => $record->export()
        ]);
    }
}
