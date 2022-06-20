<?php

namespace App\Http\Controllers;

use App\Models\Identifier;
use App\Models\Record;
use App\Models\Source;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;

class RecordController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return void
     */
    public function index(Source $source)
    {
        //
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return void
     */
    public function create(Source $source)
    {
        //
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param Source $source
     * @param Request $request
     * @return RedirectResponse
     */
    public function store(Source $source, Request $request)
    {
        $record = Record::store($source);

        return redirect()->route('sources.records.show', [$source, $record]);
    }

    /**
     * Display the specified resource.
     *
     * @param Source $source
     * @param Record $record
     * @return Application|Factory|View
     */
    public function show(Source $source, Record $record)
    {
        return view('sources.records.show', [
            'source'     => $source,
            'record'     => $record,
            'entries'    => $record->entries()->get(),
            'identifier' => Identifier::infer(),
            'can_vote'   => Identifier::infer() != $record->identifier
        ]);
    }
}
